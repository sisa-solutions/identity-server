using System.Text;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

using Sisa.Abstractions;
using Sisa.Enums;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;
using Sisa.Identity.Infrastructure.EmailTemplates;
using Sisa.Identity.Settings;

namespace Sisa.Identity.Server.V1.Account.Commands;

/// <inheritdoc/>
public class RegisterCommand : ICommand<IResult>
{
    /// <inheritdoc/>
    public string UserName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Password { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string? FirstName { get; set; }

    /// <inheritdoc/>
    public string? LastName { get; set; }

    /// <inheritdoc/>
    public string? FullName { get; set; }

    /// <inheritdoc/>
    public DateOnly? BirtDate { get; set; }

    /// <inheritdoc/>
    public Gender Gender { get; set; }

    /// <inheritdoc/>
    public bool Remember { get; set; }

    /// <inheritdoc/>
    public string ReturnUrl { get; set; } = string.Empty;

    /// <inheritdoc/>
    public bool IsPreferredEmail => UserName.Contains('@');

    /// <inheritdoc/>
    public bool IsPreferredPhone => !IsPreferredEmail;

    /// <inheritdoc/>
    public Dictionary<string, string> Ext { get; set; } = new();
}

internal class RegisterCommandHandler(
    IDataProtectionProvider dataProtectorProvider,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IEmailService emailService,
    IOptions<AppSettings> appSettingsAccessor,
    ILogger<RegisterCommand> logger
) : ICommandHandler<RegisterCommand, IResult>
{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector("Sisa.Identity.Server");

    public async ValueTask<IResult> HandleAsync(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = new User(command.UserName);

        if (command.IsPreferredEmail)
            user.SetEmail(command.UserName);
        else if (command.IsPreferredPhone)
            user.SetPhoneNumber(command.UserName);

        user.UpdatePersonalInfo(
            command.FirstName,
            command.LastName,
            command.FullName,
            command.BirtDate,
            command.Gender
        );

        user.AddOrUpdateClaims(command.Ext);

        var result = await userManager.CreateAsync(user, command.Password);

        if (result.Succeeded)
        {
            logger.LogInformation("User created a new account with password.");

            await userManager.UpdateAsync(user);

            if (command.IsPreferredEmail)
            {
                logger.LogInformation("User created a new account with email.");

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

                await signInManager.SignInAsync(user, isPersistent: false);

                logger.LogInformation("User created a new account with password.");

                return Results.Redirect(command.ReturnUrl);
            }

            if (command.IsPreferredPhone)
            {
                logger.LogInformation("User created a new account with phone number.");

                // TODO: SEND OTP CONFIRM
                throw new NotImplementedException("Register account by using phone number is not support yet.");
            }
        }

        logger.LogError("User created a new account with password failed: {@Result}", result);

        return Results.Problem(result.Errors.FirstOrDefault()?.Description);
    }
}
