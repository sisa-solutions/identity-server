using System.Collections.Immutable;
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
using Sisa.Identity.Api.V1.Connect.Responses;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Helpers;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Api;

/// <summary>
/// Represents the command to confirm the verification.
/// </summary>
public record ConfirmVerificationCommand : ICommand<IResult>
{
    /// <summary>
    /// Gets or sets the accepted.
    /// </summary>
    [FromForm]
    public bool Accepted { get; set; }
}

/// <summary>
/// Represents the handler for the <see cref="ConfirmVerificationCommand"/>.
/// </summary>
public class ConfirmVerificationCommandHandler(
    UserManager<User> userManager,
    OpenIddictScopeManager<Scope> scopeManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<ConfirmVerificationCommandHandler> logger
) : ICommandHandler<ConfirmVerificationCommand, IResult>
{
    /// <summary>
    /// Handles the <see cref="ConfirmVerificationCommand"/>.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(ConfirmVerificationCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Confirming verification.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!command.Accepted)
        {
            return TypedResults.Forbid(
                    properties: new AuthenticationProperties()
                {
                    // This property points to the address OpenIddict will automatically
                    // redirect the user to after rejecting the authorization demand.
                    RedirectUri = "/"
                }
                , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
            );
        }

        // Retrieve the profile of the logged in user.
        var user = await userManager.GetUserAsync(httpContext.User) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Retrieve the claims principal associated with the user code.
        var result = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (result.Succeeded)
        {
            // Create the claims-based identity that will be used by OpenIddict to generate tokens.
            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            // Add the claims that will be persisted in the tokens.
            identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user))
                    .SetClaim(Claims.Email, await userManager.GetEmailAsync(user))
                    .SetClaim(Claims.Name, await userManager.GetUserNameAsync(user))
                    .SetClaims(Claims.Role, (await userManager.GetRolesAsync(user)).ToImmutableArray());

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            identity.SetScopes(result.Principal.GetScopes());
            identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(OpenIddictHelper.GetDestinations);

            var properties = new AuthenticationProperties
            {
                // This property points to the address OpenIddict will automatically
                // redirect the user to after validating the authorization demand.
                RedirectUri = "/"
            };

            return TypedResults.SignIn(new ClaimsPrincipal(identity), properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Redisplay the form when the user code is not valid.
        return TypedResults.BadRequest(new VerifyResponse
        {
            Error = Errors.InvalidToken,
            ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
        });
    }
}