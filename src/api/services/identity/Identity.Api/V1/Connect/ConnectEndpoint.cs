using Sisa.Abstractions;
using Sisa.Extensions;

using Sisa.Identity.Api.V1.Connect.Commands;
using Sisa.Identity.Api.V1.Connect.Queries;

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

        var group = builder.MapGroup("/api/v1/connect");

        group.MapMethods("/authorize", ["GET", "POST"], async (AuthorizeCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapPost("/token", async (ExchangeTokenCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapGet("/consent", async (GetConsentInfoQuery query, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(query, cancellationToken)
        );

        group.MapPost("/consent", async (ConfirmConsentCommand command, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(command, cancellationToken)
        );

        group.MapGet("/error", async (GetErrorQuery query, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(query, cancellationToken)
        );

        group.MapGet("/verify", async (GetVerificationInfoQuery query, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(query, cancellationToken)
        );

        group.MapPost("/verify", async (ConfirmVerificationCommand query, CancellationToken cancellationToken = default) =>
            await mediator.SendAsync(query, cancellationToken)
        );



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
