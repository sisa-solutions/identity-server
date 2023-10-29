using Sisa.Abstractions;
using Sisa.Extensions;

using Sisa.Identity.Api.V1.Connect.Commands;

namespace Sisa.Identity.Api.V1.Connect;

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

        var group = builder.MapGroup("/api/v1/Connect");

        group.MapMethods("/login", ["GET", "POST"],
            async ([AsParameters] AuthorizeCommand command, CancellationToken cancellationToken = default) => await mediator.SendAsync(command, cancellationToken));

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
