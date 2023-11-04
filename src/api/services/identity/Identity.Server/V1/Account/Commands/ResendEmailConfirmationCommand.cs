using System.Text;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;
using Sisa.Identity.Infrastructure.EmailTemplates;
using Sisa.Identity.Settings;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <inheritdoc/>
public class ResendEmailConfirmationCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    public string UserName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public bool Remember { get; set; }

    /// <inheritdoc/>
    public string ReturnUrl { get; set; } = string.Empty;

    /// <inheritdoc/>
    public bool IsPreferredEmail => UserName.Contains('@');

    /// <inheritdoc/>
    public bool IsPreferredPhone => !IsPreferredEmail;

    /// <inheritdoc/>
    public Dictionary<string, string> Ext { get; set; } = [];
}

internal class ResendEmailConfirmationCommandHandler(
    IDataProtectionProvider dataProtectorProvider,
    UserManager<User> userManager,
    IEmailService emailService,
    IOptions<AppSettings> appSettingsAccessor,
    ILogger<ResendEmailConfirmationCommand> logger
) : ICommandHandler<ResendEmailConfirmationCommand, IResult>
{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector("Sisa.Identity.Server");

    public async ValueTask<IResult> HandleAsync(
        ResendEmailConfirmationCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.IsPreferredEmail)
        {
            var user = await userManager.FindByEmailAsync(command.UserName);

            if (user == null)
            {
                logger.LogError("User not found");

                return Results.Problem("Verification email sent. Please check your email.");
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = _dataProtector.Protect(Encoding.UTF8.GetBytes($"CONFIRM_EMAIL:{user.Id}:{code}"));
            var token = WebEncoders.Base64UrlEncode(tokenBytes);
            var confirmEmailUrl = $"{appSettingsAccessor.Value.Identity.Authority}/confirm-email?return_url={command.ReturnUrl}&token={token}";

            logger.LogDebug("confirmEmailUrl: {0}", confirmEmailUrl);

            var confirmEmailModel = new RegistrationConfirmModel(
                appSettingsAccessor.Value.Email.Contact.Company,
                user.FullName ?? user.Email!,
                code,
                confirmEmailUrl
            );

            if (!await emailService.SendRegistrationConfirmAsync(user.Email!, confirmEmailModel, cancellationToken: cancellationToken))
            {
                logger.LogError("Error sending confirm email");

                return Results.Problem("Error sending confirm email");
            }

            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                logger.LogInformation("User created a new account with email and need to confirm email.");

                return Results.LocalRedirect($"/confirm-email?return_url={command.ReturnUrl}&token={token}&source=web-link");
            }

            return Results.NoContent();
        }

        if (command.IsPreferredPhone)
        {
            logger.LogInformation("User created a new account with phone number.");

            // TODO: SEND OTP CONFIRM
            throw new NotImplementedException("Register account by using phone number is not support yet.");
        }

        return Results.Problem();
    }
}
