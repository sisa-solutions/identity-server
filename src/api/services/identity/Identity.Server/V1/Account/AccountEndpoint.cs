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

        // group.MapPost("/logout", LogoutAsync);
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
