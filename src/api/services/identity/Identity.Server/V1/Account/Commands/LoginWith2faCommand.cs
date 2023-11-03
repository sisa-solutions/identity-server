using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Sisa.Abstractions;
using Sisa.Helpers;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <summary>
/// Represents a request to login.
/// </summary>
public class LoginWith2faCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    public string TwoFactorCode { get; set; } = null!;

    /// <inheritdoc/>
    public bool RememberMachine { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [FromQuery(Name = "remember_me")]
    public bool RememberMe { get; set; }

    /// <summary>
    /// Gets or sets the URL to return to after login.
    /// </summary>
    [FromQuery(Name = "return_url")]
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// Represents a command to login.
/// </summary>
public sealed class LoginWith2faCommandHandler(
    SignInManager<User> signInManager,
    ILogger<LoginWith2faCommandHandler> logger
) : ICommandHandler<LoginWith2faCommand, IResult>
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<IResult> HandleAsync(LoginWith2faCommand command, CancellationToken cancellationToken = default)
    {
        var returnUrl = !string.IsNullOrEmpty(command.ReturnUrl) ? Uri.UnescapeDataString(command.ReturnUrl) : string.Empty;

        if (string.IsNullOrEmpty(returnUrl))
        {
            logger.LogError("No return URL was specified.");

            throw new DomainException(StatusCodes.Status400BadRequest, "no_return_url", "Your login attempt was unsuccessful. Please try again from a trusted source.");
        }

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
        {
            logger.LogError("Unable to load two-factor authentication user.");

            return Results.BadRequest("Unable to load two-factor authentication user.");
        }

        var authenticatorCode = command.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, command.RememberMe, command.RememberMachine);

        if (result.Succeeded)
        {
            // request for a local page
            if (UrlHelper.IsLocalUrl(returnUrl))
            {
                logger.LogInformation("User logged in with 2fa.");

                return Results.LocalRedirect(returnUrl);
            }

            logger.LogInformation("Return url is not local");
        }

        if (result.IsLockedOut)
        {
            logger.LogWarning("User account locked out.");

            return Results.LocalRedirect("/lockout");
        }

        logger.LogWarning("Invalid authenticator code entered.");

        return Results.BadRequest("Invalid authenticator code entered.");
    }
}
