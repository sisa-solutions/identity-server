using Sisa.Abstractions;
using Sisa.Identity.Infrastructure.Abstractions;
using Sisa.Identity.Infrastructure.EmailTemplates;

namespace Sisa.Identity.Infrastructure.Services;

[SingletonService]
public class EmailService(
    IEmailSenderService emailSender
    ) : IEmailService
{
    public async Task<bool> SendRegistrationConfirmAsync(string to, RegistrationConfirmModel model, CancellationToken cancellationToken = default)
    {
        var templateName = "RegistrationConfirm.cshtml";

        return await emailSender
            .SendWithEmbeddedTemplateAsync(to, "Confirm Your Email Address to Activate Your Account", model, templateName, cancellationToken);
    }

    public async Task<bool> SendResetPasswordAsync(string to, ResetPasswordModel model, CancellationToken cancellationToken = default)
    {
        var templateName = "ResetPassword.cshtml";

        return await emailSender
            .SendWithEmbeddedTemplateAsync(to, "Reset Your Password", model, templateName, cancellationToken);
    }

    public async Task<bool> SendTwoFactorAuthenticationTokenAsync(string to, TwoFactorAuthenticationModel model, CancellationToken cancellationToken = default)
    {
        var templateName = "TwoFactorAuthenticationToken.cshtml";

        return await emailSender
            .SendWithEmbeddedTemplateAsync(to, "Your 2FA Token", model, templateName, cancellationToken);
    }

    public async Task<bool> SendAccountInformationAsync(string to, AccountInformationModel model, CancellationToken cancellationToken = default)
    {
        var templateName = "AccountInformation.cshtml";

        return await emailSender
            .SendWithEmbeddedTemplateAsync(to, "Your Account Information", model, templateName, cancellationToken);
    }

    public async Task<bool> SendBlockAccountInfomationAsync(string to, BlockAccountModel model, CancellationToken cancellationToken = default)
    {
        var templateName = "BlockAccount.cshtml";

        return await emailSender
            .SendWithEmbeddedTemplateAsync(to, "Your Account Has Been Locked", model, templateName, cancellationToken);
    }
}
