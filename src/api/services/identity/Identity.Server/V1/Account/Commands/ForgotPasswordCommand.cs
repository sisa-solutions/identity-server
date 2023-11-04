using System.Text;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Sisa.Abstractions;
using Sisa.Enums;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;
using Sisa.Identity.Infrastructure.EmailTemplates;
using Sisa.Identity.Settings;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <inheritdoc/>
public class ForgotPasswordCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    public string ReturnUrl { get; set; } = null!;

    /// <inheritdoc/>
    public string UserName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public VerifyMethod VerifyMethod { get; set; }

    /// <inheritdoc/>
    public bool IsPreferredEmail => UserName.Contains('@');

    /// <inheritdoc/>
    public bool IsPreferredPhone => !IsPreferredEmail;
}

/// <summary>
/// Represents a command to login.
/// </summary>
internal class ForgotPasswordCommandHandler(
    IDataProtectionProvider dataProtectorProvider,
    UserManager<User> userManager,
    IEmailService emailService,
    IOptions<AppSettings> appSettingsAccessor,
    ILogger<ForgotPasswordCommand> logger
) : ICommandHandler<ForgotPasswordCommand, IResult>
{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector("Sisa.Identity.Server");

    public async ValueTask<IResult> HandleAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation("Forgot password");

        User? user = null;

        if (command.IsPreferredEmail)
            user = await userManager.FindByEmailAsync(command.UserName);
        else if (command.IsPreferredPhone)
            user = await userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == command.UserName, cancellationToken);

        // var response = new RedirectResponse()
        // {
        //     RedirectUrl = "/forgot-password-confirmation"
        // };

        // Don't reveal that the user does not exist
        if (user == null)
        {
            logger.LogInformation("User with email {Email} not found", command.UserName);

            return Results.Redirect("/forgot-password-confirmation");
        }

        if (command.IsPreferredEmail)
        {
            logger.LogInformation("User with email {Email} found", command.UserName);

            bool isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);

            // Don't reveal that the user is confirmed
            if (!isEmailConfirmed)
            {
                logger.LogInformation("User with email {Email} is not confirmed", command.UserName);

                return Results.Redirect("/forgot-password-confirmation");
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var tokenBytes = _dataProtector.Protect(Encoding.UTF8.GetBytes($"RESET_PASSWORD:{user.Id}:{code}"));
            var token = WebEncoders.Base64UrlEncode(tokenBytes);
            var resetPasswordUrl = $"{appSettingsAccessor.Value.Identity.Authority}/reset-password?return_url={command.ReturnUrl}&token={token}&source=email-link";

            logger.LogDebug("resetPasswordUrl: {0}", resetPasswordUrl);

            var resetPasswordModel = new ResetPasswordModel(
                appSettingsAccessor.Value.Email.Contact.Company,
                appSettingsAccessor.Value.Email.Contact.Name,
                appSettingsAccessor.Value.Email.Contact.Email,
                user.FullName ?? user.Email!,
                resetPasswordUrl);

            if (!await emailService.SendResetPasswordAsync(user.Email!, resetPasswordModel, cancellationToken: cancellationToken))
            {
                logger.LogError("Failed to send reset password email to {Email}", user.Email);

                return Results.BadRequest("Failed to send reset password email");
            }
        }
        else if (command.IsPreferredPhone)
        {
            logger.LogInformation("User with phone number {PhoneNumber} found", command.UserName);

            bool isPhoneNumberConfirmed = await userManager.IsPhoneNumberConfirmedAsync(user);

            throw new NotImplementedException("Verify by using Phone Number is not support yet.");
        }

        return Results.Redirect($"/forgot-password-confirmation");
    }
}
