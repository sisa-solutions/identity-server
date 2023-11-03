using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Sisa.Abstractions;
using Sisa.Helpers;
using Sisa.Identity.Server.V1.Account.Responses;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <summary>
/// Represents a request to login.
/// </summary>
public class LoginCommand : ICommand<RedirectResponse>
{
    /// <summary>
    /// Gets or sets the username or email address.
    /// </summary>
    [FromForm]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [FromForm]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user wants to stay signed in.
    /// </summary>
    [FromForm]
    public bool RememberMe { get; set; } = false;

    /// <summary>
    /// Gets or sets the URL to return to after login.
    /// </summary>
    [FromQuery(Name = "return_url")]
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// Represents a command to login.
/// </summary>
public sealed class LoginCommandHandler(
    SignInManager<User> signInManager,
    ILogger<LoginCommandHandler> logger
) : ICommandHandler<LoginCommand, RedirectResponse>
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<RedirectResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var returnUrl = !string.IsNullOrEmpty(command.ReturnUrl) ? Uri.UnescapeDataString(command.ReturnUrl) : string.Empty;

        if (string.IsNullOrEmpty(returnUrl))
        {
            logger.LogError("No return URL was specified.");

            throw new DomainException(StatusCodes.Status400BadRequest, "no_return_url", "Your login attempt was unsuccessful. Please try again from a trusted source.");
        }

        RedirectResponse redirectResponse = new()
        {
            RedirectUrl = returnUrl
        };

        var loginResult = await signInManager.PasswordSignInAsync(command.Username, command.Password, command.RememberMe, lockoutOnFailure: true);

        if (loginResult.Succeeded)
        {
            logger.LogInformation("User logged in.");

            if (UrlHelper.IsLocalUrl(returnUrl))
            {
                logger.LogInformation("Redirecting to {returnUrl}.", returnUrl);

                return redirectResponse;
            }

            logger.LogError("Invalid return URL was specified.");

            await signInManager.SignOutAsync();

            throw new DomainException(StatusCodes.Status400BadRequest, "invalid_return_url", "You might have clicked on a malicious link - logged out now");
        }

        if (loginResult.RequiresTwoFactor)
        {
            logger.LogWarning("Two-factor authentication is required.");

            redirectResponse.RedirectUrl = $"/login-with-2fa?return_url={returnUrl}&remember_mne={command.RememberMe}";

            return redirectResponse;
        }

        if (loginResult.IsLockedOut)
        {
            logger.LogWarning("The user account is locked out.");

            redirectResponse.RedirectUrl = "/lockout";

            return redirectResponse;
        }

        logger.LogError("Invalid credentials were specified.");

        await Task.CompletedTask;

        throw new DomainException(StatusCodes.Status400BadRequest, "invalid_credentials", "Your login attempt was unsuccessful. Please try again from a trusted source.");
    }
}
