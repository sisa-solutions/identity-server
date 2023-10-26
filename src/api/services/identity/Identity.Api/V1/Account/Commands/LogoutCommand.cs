using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Sisa.Abstractions;
using Sisa.Identity.Api.V1.Account.Responses;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Api;

/// <summary>
/// Represents a command to logout.
/// </summary>
public class LogoutCommand : ICommand<RedirectResponse>
{
    /// <summary>
    /// Gets or sets the URL to return to after login.
    /// </summary>
    [FromQuery(Name = "return_url")]
    public string? ReturnUrl { get; set; }
}


/// <summary>
/// Represents a command to login.
/// </summary>
public sealed class LogoutCommandHandler(
    // SignInManager<User> signInManager,
    ILogger<LogoutCommandHandler> logger
) : ICommandHandler<LogoutCommand, RedirectResponse>
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<RedirectResponse> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        // await signInManager.SignOutAsync();

        await Task.CompletedTask;

        logger.LogInformation("Logging out user.");

        RedirectResponse response = new()
        {
            RedirectUrl = command.ReturnUrl ?? "/"
        };

        return response;
    }
}
