using System.Text;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <inheritdoc/>
public class ResetPasswordCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    public string ReturnUrl { get; set; } = null!;

    /// <inheritdoc/>
    public string Email { get; set; } = null!;

    /// <inheritdoc/>
    public string Password { get; set; } = null!;

    /// <inheritdoc/>
    [FromQuery(Name = "token")]
    public string Token { get; set; } = null!;
}

/// <inheritdoc/>
public sealed class ResetPasswordCommandHandler(
    IDataProtectionProvider dataProtectorProvider,
    UserManager<User> userManager,
    ILogger<ResetPasswordCommandHandler> logger
) : ICommandHandler<ResetPasswordCommand, IResult>
{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector("Sisa.Identity.Server");

    /// <inheritdoc/>
    public async ValueTask<IResult> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
    {
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

        if(user.Email != command.Email)
        {
            logger.LogError("Invalid token: {Token}", command.Token);

            return TypedResults.BadRequest("Invalid token");
        }

        // var oldPasswordHash = user.PasswordHash;

        var result = await userManager.ResetPasswordAsync(user, tokenParts[2], command.Password);

        if (!result.Succeeded)
        {
            logger.LogError("An error occur during process Reset password: {@Result}", result);

            throw new Exception("An error occur during process Reset password.");
        }

        result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("An error occur during process Reset password: {@Result}", result);

            throw new Exception("An error occur during process Reset password.");
        }

        logger.LogInformation("User with email {Email} reset password", command.Email);

        return Results.Redirect($"/reset-password-confirmation?return_url={command.ReturnUrl}");
    }
}
