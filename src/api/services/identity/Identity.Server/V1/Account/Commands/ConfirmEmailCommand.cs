using System.Text;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <summary>
/// Represents a command to ConfirmEmail.
/// </summary>
public class ConfirmEmailCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    [FromQuery(Name = "token")]
    public string Token { get; set; } = null!;
}

/// <summary>
/// Represents a command to login.
/// </summary>
public sealed class ConfirmEmailCommandHandler(
    IDataProtectionProvider dataProtectorProvider,
    UserManager<User> userManager,
    ILogger<ConfirmEmailCommand> logger
) : ICommandHandler<ConfirmEmailCommand, IResult>
{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector("Sisa.Identity.Server");

    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async ValueTask<IResult> HandleAsync(ConfirmEmailCommand command, CancellationToken cancellationToken = default)
    {
        // format: PURPOSE(CONFIRM_EMAIL|CONFIRM_PHONE_NUMBER|RESET_PASSWORD|CHANGE_PASSWORD|...):USER_ID:TOKEN

        logger.LogInformation("Confirm email for user");

        var tokenBytes = _dataProtector.Unprotect(WebEncoders.Base64UrlDecode(command.Token));
        var token = Encoding.UTF8.GetString(tokenBytes);

        var tokenParts = token.Split(':');

        if (tokenParts.Length != 3)
        {
            logger.LogError("Invalid token: {Token}", command.Token);

            return TypedResults.BadRequest("Invalid token");
        }

        if ("CONFIRM_EMAIL".Equals(tokenParts[0], StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("Invalid token: {Token}", command.Token);

            return TypedResults.BadRequest("Invalid token");
        }

        if (!Guid.TryParse(tokenParts[1], out var userId))
        {
            logger.LogError("Invalid token: {Token}", command.Token);

            return TypedResults.BadRequest("Invalid token");
        }

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            logger.LogError("User with id {Id} not found", userId);

            return TypedResults.BadRequest("Invalid token");
        }

        var result = await userManager.ConfirmEmailAsync(user, tokenParts[2]);

        if (!result.Succeeded)
        {
            logger.LogError("An error occur during process ConfirmEmailRequest: {@Errors}", result.Errors);

            return TypedResults.BadRequest(result.Errors);
        }

        logger.LogInformation("User with id {Id} confirmed email", user.Id);

        return TypedResults.NoContent();
    }
}
