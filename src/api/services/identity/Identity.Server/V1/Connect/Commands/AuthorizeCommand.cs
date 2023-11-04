using System.Security.Claims;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Core;
using OpenIddict.Server.AspNetCore;

using Sisa.Abstractions;
using Sisa.Extensions;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Helpers;
using Sisa.Identity.Server.V1.Connect.Queries;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Server.V1.Connect.Commands;

/// <summary>
/// Authorize command.
/// </summary>
public class AuthorizeCommand : ICommand<IResult>
{

}

/// <summary>
/// Authorize command handler.
/// </summary>
public class AuthorizeCommandHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    OpenIddictApplicationManager<Application> applicationManager,
    OpenIddictAuthorizationManager<Authorization> authorizationManager,
    OpenIddictScopeManager<Scope> scopeManager,
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator,
    ILogger<AuthorizeCommandHandler> logger
) : ICommandHandler<AuthorizeCommand, IResult>
{
    /// <summary>
    /// Handle authorize command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<IResult> HandleAsync(AuthorizeCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Authorize command handler executed.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Try to retrieve the user principal stored in the authentication cookie and redirect
        // the user agent to the login page (or to an external provider) in the following cases:
        //
        //  - If the user principal can't be extracted or the cookie is too old.
        //  - If prompt=login was specified by the client application.
        //  - If a max_age parameter was provided and the authentication cookie is not considered "fresh" enough.
        //
        // For scenarios where the default authentication handler configured in the ASP.NET Core
        // authentication options shouldn't be used, a specific scheme can be specified here.
        var result = await httpContext.AuthenticateAsync();

        if (result == null || !result.Succeeded || request.HasPrompt(Prompts.Login) ||
           (request.MaxAge != null && result.Properties?.IssuedUtc != null &&
            DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value)))
        {
            // If the client application requested promptless authentication,
            // return an error indicating that the user is not logged in.
            if (request.HasPrompt(Prompts.None))
            {
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                    }),
                    authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );
            }

            // To avoid endless login -> authorization redirects, the prompt=login flag
            // is removed from the authorization request payload before redirecting the user.
            var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));

            var parameters = httpContext.Request.HasFormContentType ?
                httpContext.Request.Form.Where(parameter => parameter.Key != Parameters.Prompt).ToList() :
                httpContext.Request.Query.Where(parameter => parameter.Key != Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

            var returnUrl = $"{httpContext.Request.PathBase}{httpContext.Request.Path}{BuildQueryString(httpContext, request)}";

            // For applications that want to allow the client to select the external authentication provider
            // that will be used to authenticate the user, the identity_provider parameter can be used for that.
            if (!string.IsNullOrEmpty(request.IdentityProvider))
            {
                // if (!string.Equals(request.IdentityProvider, Providers.GitHub, StringComparison.Ordinal))
                // {
                //     return Forbid(
                //         authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                //         properties: new AuthenticationProperties(new Dictionary<string, string>
                //         {
                //             [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidRequest,
                //             [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                //                 "The specified identity provider is not valid."
                //         }));
                // }

                var properties = signInManager.ConfigureExternalAuthenticationProperties(
                    provider: request.IdentityProvider,
                    redirectUrl: $"/api/v1/account/external-login-callback?return_url={returnUrl}"
                );
                // Note: when only one client is registered in the client options,
                // specifying the issuer URI or the provider name is not required.
                properties.SetString(OpenIddictClientAspNetCoreConstants.Properties.ProviderName, request.IdentityProvider);

                // Ask the OpenIddict client middleware to redirect the user agent to the identity provider.
                return TypedResults.Challenge(properties, [OpenIddictClientAspNetCoreDefaults.AuthenticationScheme]);
            }

            // For scenarios where the default challenge handler configured in the ASP.NET Core
            // authentication options shouldn't be used, a specific scheme can be specified here.
            return TypedResults.Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl,
                }
                // , new[] { IdentityConstants.ApplicationScheme }
            );
        }

        // Retrieve the profile of the logged in user.
        var user = await userManager.GetUserAsync(result.Principal) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the application details from the database.
        var application = await applicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty, cancellationToken) ??
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        var requestScopes = request.GetScopes();

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await authorizationManager.FindAsync(
            subject: await userManager.GetUserIdAsync(user)
            , client: application.Id.ToString()
            , status: Statuses.Valid
            , type: AuthorizationTypes.Permanent
            , scopes: requestScopes
            , cancellationToken: cancellationToken)
        .ToListAsync();

        var isAuthorizationExist = authorizations.Count != 0;

        switch (await applicationManager.GetConsentTypeAsync(application, cancellationToken))
        {
            // If the consent is external (e.g when authorizations are granted by a sysadmin),
            // immediately return an error if no authorization can be found in the database.
            case ConsentTypes.External when !isAuthorizationExist:
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }),
                    authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );

            // If the consent is implicit or if an authorization was found,
            // return an authorization response without displaying the consent form.
            case ConsentTypes.Implicit:
            case ConsentTypes.External when isAuthorizationExist:
            case ConsentTypes.Explicit when isAuthorizationExist && !request.HasPrompt(Prompts.Consent):
                // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);

                // Add the claims that will be persisted in the tokens.
                var userInfo = await mediator.SendAsync(new GetUserInfoQuery
                {
                    Id = user.Id.ToString()
                }, cancellationToken);

                if (userInfo is not null)
                {
                    foreach (var item in userInfo)
                    {
                        identity.SetClaim(item.Key, item.Value.ToString());
                    }
                }

                // Note: in this sample, the granted scopes match the requested scope
                // but you may want to allow the user to uncheck specific scopes.
                // For that, simply restrict the list of scopes before calling SetScopes.
                identity.SetScopes(request.GetScopes());
                identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes(), cancellationToken).ToListAsync());

                // Automatically create a permanent authorization to avoid requiring explicit consent
                // for future authorization or token requests containing the same scopes.
                var authorization = authorizations.LastOrDefault();

                authorization ??= await authorizationManager.CreateAsync(
                    identity: identity
                    , subject: await userManager.GetUserIdAsync(user)
                    , client: application.Id.ToString()
                    , type: AuthorizationTypes.Permanent
                    , scopes: identity.GetScopes()
                    , cancellationToken: cancellationToken);

                identity.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization, cancellationToken));
                identity.SetDestinations(OpenIddictHelper.GetDestinations);

                return TypedResults.SignIn(new ClaimsPrincipal(identity), authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // At this point, no authorization was found in the database and an error must be returned
            // if the client application specified prompt=none in the authorization request.
            case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
            case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "Interactive user consent is required."
                    }),
                    authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );

            // In every other case, render the consent form.
            default:
                return TypedResults.LocalRedirect($"{httpContext.Request.PathBase}/consent{BuildQueryString(httpContext, request)}");
        }
    }

    private QueryString BuildQueryString(HttpContext httpContext, OpenIddictRequest request)
    {
        // To avoid endless login -> authorization redirects, the prompt=login flag
        // is removed from the authorization request payload before redirecting the user.
        var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));

        var parameters = httpContext.Request.HasFormContentType ?
            httpContext.Request.Form.Where(parameter => parameter.Key != Parameters.Prompt).ToList() :
            httpContext.Request.Query.Where(parameter => parameter.Key != Parameters.Prompt).ToList();

        var openIddictParameters = request.GetParameters();

        foreach (var parameter in openIddictParameters)
        {
            var parameterValue = parameter.Value.Value?.ToString();

            if (!string.IsNullOrEmpty(parameterValue))
                parameters.Add(KeyValuePair.Create(parameter.Key, new StringValues(parameterValue)));
        }

        parameters.Add(KeyValuePair.Create(Parameters.Prompt, new StringValues(prompt)));

        var extraParams = httpContext.Request.Query
                   .Where(x => x.Key.StartsWith("ext_", StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Key, x => x.Value.ToString());

        foreach (var extraParam in extraParams)
        {
            parameters.Add(KeyValuePair.Create(extraParam.Key, new StringValues(extraParam.Value)));
        }

        return QueryString.Create(parameters.Distinct());
    }
}
