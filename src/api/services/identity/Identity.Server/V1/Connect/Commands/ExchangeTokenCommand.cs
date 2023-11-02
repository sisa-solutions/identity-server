using System.Security.Claims;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
/// Represents the command to exchange a token.
/// </summary>
public record ExchangeTokenCommand : ICommand<IResult>
{

}

/// <summary>
/// Represents the handler for the <see cref="ExchangeTokenCommand"/>.
/// </summary>
public class ExchangeTokenCommandHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    OpenIddictScopeManager<Scope> scopeManager,
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator,
    ILogger<AuthorizeCommandHandler> logger) : ICommandHandler<ExchangeTokenCommand, IResult>
{
    /// <summary>
    /// Handles the <see cref="ExchangeTokenCommand"/>.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(ExchangeTokenCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Exchange token command received.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsPasswordGrantType())
        {
            logger.LogDebug("Password grant type received.");

            var user = await userManager.FindByNameAsync(request.Username!);

            if (user is null)
            {
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The username/password couple is invalid."
                    })
                    , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );
            }

            // Validate the username/password parameters and ensure the account is not locked out.
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password!, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.RequiresTwoFactor)
                {
                    logger.LogWarning("Two-factor authentication is required.");

                    return TypedResults.Forbid(
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Two-factor authentication is required."
                        })
                        , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                    );
                }

                if (result.IsLockedOut)
                {
                    logger.LogWarning("The user account is locked out.");

                    return TypedResults.Forbid(
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user account is locked out."
                        })
                        , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                    );
                }

                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid credentials were specified."
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
            var userInfo = await mediator.SendAsync(new GetUserInfoQuery(), cancellationToken);

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
            identity.SetDestinations(OpenIddictHelper.GetDestinations);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return TypedResults.SignIn(new ClaimsPrincipal(identity), authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        else if (request.IsAuthorizationCodeGrantType() || request.IsDeviceCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            logger.LogDebug("Authorization code/device code/refresh token grant type received.");

            // Retrieve the claims principal stored in the authorization code/device code/refresh token.
            var result = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Retrieve the user profile corresponding to the authorization code/refresh token.
            var user = await userManager.FindByIdAsync(result.Principal?.GetClaim(Claims.Subject) ?? string.Empty);

            if (user is null)
            {
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                    })
                    , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );
            }

            // Ensure the user is still allowed to sign in.
            if (!await signInManager.CanSignInAsync(user))
            {
                return TypedResults.Forbid(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                    })
                    , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );
            }

            var identity = new ClaimsIdentity(result.Principal?.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role
            );

            // Override the user claims present in the principal in case they
            // changed since the authorization code/refresh token was issued.
            var userInfo = await mediator.SendAsync(new GetUserInfoQuery(), cancellationToken);

            if (userInfo is not null)
            {
                foreach (var item in userInfo)
                {
                    identity.SetClaim(item.Key, item.Value.ToString());
                }
            }

            identity.SetDestinations(OpenIddictHelper.GetDestinations);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return TypedResults.SignIn(new ClaimsPrincipal(identity), authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        logger.LogDebug("Unsupported grant type received.");

        throw new InvalidOperationException("The specified grant type is not supported.");
    }
}
