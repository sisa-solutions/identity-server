using System.Security.Claims;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using OpenIddict.Abstractions;
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
/// Represents the command to confirm the consent.
/// </summary>
public record ConfirmConsentCommand : ICommand<IResult>
{
    /// <summary>
    /// Gets or sets the accepted.
    /// </summary>
    [FromForm]
    public bool Accepted { get; set; }
}

/// <summary>
/// Represents the handler for the <see cref="ConfirmConsentCommand"/>.
/// </summary>
public class ConfirmConsentCommandHandler(
    UserManager<User> userManager,
    OpenIddictApplicationManager<Application> applicationManager,
    OpenIddictAuthorizationManager<Authorization> authorizationManager,
    OpenIddictScopeManager<Scope> scopeManager,
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator,
    ILogger<AuthorizeCommandHandler> logger
) : ICommandHandler<ConfirmConsentCommand, IResult>
{
    /// <summary>
    /// Handles the <see cref="ConfirmConsentCommand"/>.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(ConfirmConsentCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Confirming consent.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!command.Accepted)
        {
            return TypedResults.Forbid(
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.AccessDenied,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The consent was not accepted by the logged in user."
                })
                , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
            );
        }

        // Retrieve the profile of the logged in user.
        User user = await userManager.GetUserAsync(httpContext.User) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the application details from the database.
        Application application = await applicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty, cancellationToken) ??
            throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await authorizationManager.FindAsync(
            subject: await userManager.GetUserIdAsync(user)
            , client: application.ClientId ?? string.Empty
            , status: Statuses.Valid
            , type: AuthorizationTypes.Permanent
            , scopes: request.GetScopes()
            , cancellationToken: cancellationToken
        ).ToListAsync();

        // Note: the same check is already made in the other action but is repeated
        // here to ensure a malicious user can't abuse this POST-only endpoint and
        // force it to return a valid response without the external authorization.
        if (!authorizations.Any() && await applicationManager.HasConsentTypeAsync(application, ConsentTypes.External, cancellationToken))
        {
            return TypedResults.Forbid(
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The logged in user is not allowed to access this client application."
                })
                , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
            );
        }

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
            , client: application.ClientId ?? string.Empty
            , type: AuthorizationTypes.Permanent
            , scopes: identity.GetScopes()
            , cancellationToken: cancellationToken
        );

        identity.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization, cancellationToken));
        identity.SetDestinations(OpenIddictHelper.GetDestinations);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return TypedResults.SignIn(new ClaimsPrincipal(identity), authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}