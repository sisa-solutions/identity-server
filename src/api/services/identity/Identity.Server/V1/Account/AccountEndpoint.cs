using Sisa.Abstractions;
using Sisa.Extensions;

using Sisa.Identity.Server.V1.Account.Commands;
using Sisa.Identity.Server.V1.Account.Responses;

namespace Sisa.Identity.Server.V1.Account;

/// <summary>
/// Represents the account endpoint.
/// </summary>
public static class AccountEndpoint
{
    /// <summary>
    /// Handles a request to login.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static RouteGroupBuilder MapAccountEndpoint(this IEndpointRouteBuilder builder)
    {
        var mediator = builder.ServiceProvider.GetRequiredService<IMediator>();

        var group = builder.MapGroup("/api/v1/account");

        group.MapPost("/login", async ([AsParameters] LoginCommand command, CancellationToken cancellationToken = default) =>
        {
            var response = await mediator.SendAsync(command, cancellationToken);

            if (!string.IsNullOrEmpty(response.RedirectUrl))
            {
                TypedResults.Redirect(response.RedirectUrl);
            }

            return TypedResults.Ok(response);
        })
        .Produces<RedirectResponse>(StatusCodes.Status200OK);

        group.MapPost("/login-with-2fa", async ([AsParameters] LoginWith2faCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken));

        group.MapPost("/login-with-recovery-code", async ([AsParameters] LoginWithRecoveryCodeCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken));

        // group.MapPost("/logout", LogoutAsync);
        group.MapPost("/register", async ([AsParameters] RegisterCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapPost("/confirm-email", async ([AsParameters] ConfirmEmailCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapPost("/forgot-password", async ([AsParameters] ForgotPasswordCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapPost("/reset-password", async ([AsParameters] ResetPasswordCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken));

        // group.MapPost("/change-password", ChangePasswordAsync);
        // group.MapPost("/change-email", ChangeEmailAsync);
        // group.MapPost("/resend-email-confirmation", ResendEmailConfirmationAsync);
        return group;
    }
}
