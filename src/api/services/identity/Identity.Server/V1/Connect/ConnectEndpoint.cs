using Microsoft.AspNetCore.Authentication;

using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using Sisa.Abstractions;
using Sisa.Extensions;

using Sisa.Identity.Server.V1.Connect.Commands;
using Sisa.Identity.Server.V1.Connect.Queries;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Server.V1.Connect;

/// <summary>
/// Represents the Connect endpoint.
/// </summary>
public static class ConnectEndpoint
{
    /// <summary>
    /// Handles a request to login.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteGroupBuilder MapConnectEndpoint(this IEndpointRouteBuilder builder)
    {
        var mediator = builder.ServiceProvider.GetRequiredService<IMediator>();

        var group = builder.MapGroup("/connect");

        group.MapMethods("/authorize", ["GET", "POST"], async (CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(new AuthorizeCommand(), cancellationToken)
        );

        group.MapPost("/token", async (CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(new ExchangeTokenCommand(), cancellationToken)
        );

        group.MapGet("/consent", async (CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(new GetConsentInfoQuery(), cancellationToken)
        );

        group.MapPost("/consent", async ([AsParameters] ConfirmConsentCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapGet("/error", async (CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(new GetErrorQuery(), cancellationToken)
        );

        group.MapGet("/verify", async (CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(new GetVerificationInfoQuery(), cancellationToken)
        );

        group.MapPost("/verify", async ([AsParameters] ConfirmVerificationCommand query, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(query, cancellationToken)
        );

        group.MapPost("/userinfo", async (HttpContext httpContext, CancellationToken cancellationToken = default) =>
        {
            var response = await mediator.SendAsync(new GetUserInfoQuery
            {
                Id = httpContext.User.GetClaim(Claims.Subject) ?? string.Empty
            }, cancellationToken);

            if (response is null)
            {
                return Results.Challenge(
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    })
                    , authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
                );
            }

            return TypedResults.Ok(response);
        });


        // group.MapPost("/register", RegisterAsync);
        // group.MapPost("/confirm-email", ConfirmEmailAsync);
        // group.MapPost("/forgot-password", ForgotPasswordAsync);
        // group.MapPost("/reset-password", ResetPasswordAsync);
        // group.MapPost("/change-password", ChangePasswordAsync);
        // group.MapPost("/change-email", ChangeEmailAsync);
        // group.MapPost("/resend-email-confirmation", ResendEmailConfirmationAsync);

        return group;
    }
}
