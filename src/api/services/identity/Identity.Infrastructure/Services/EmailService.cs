using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;
using Sisa.Identity.Infrastructure.EmailTemplates;

namespace Sisa.Identity.Infrastructure.Services;

[SingletonService]
public class EmailService : IEmailService
{
    private readonly IEmailSender<User> _emailSender;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IEmailSender<User> emailSender,
        ILogger<EmailService> logger
    )
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public Task<bool> SendAccountInformationAsync(string to, AccountInformationModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendBlockAccountInfomationAsync(string to, BlockAccountModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendConfirmEmailAsync(string to, ConfirmEmailModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendResetPasswordAsync(string to, ResetPasswordModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendTwoFactorAuthenticationTokenAsync(string to, TwoFactorAuthenticationModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    // public async Task<bool> SendConfirmEmailAsync(string to, ConfirmEmailModel model, CancellationToken cancellationToken = default)
    // {
    //     var templateName = "ConfirmEmail.cshtml";

    //     return await _emailSender
    //         .SendWithEmbeddedTemplateAsync(to, "Confirm Your Email Address to Activate Your Account", model, this.GetType().Assembly, templateName, cancellationToken);
    // }

    // public async Task<bool> SendResetPasswordAsync(string to, ResetPasswordModel model, CancellationToken cancellationToken = default)
    // {
    //     var templateName = "ResetPassword.cshtml";

    //     return await _emailSender
    //         .SendWithEmbeddedTemplateAsync(to, "Reset Your Password", model, this.GetType().Assembly, templateName, cancellationToken);
    // }

    // public async Task<bool> SendTwoFactorAuthenticationTokenAsync(string to, TwoFactorAuthenticationModel model, CancellationToken cancellationToken = default)
    // {
    //     var templateName = "TwoFactorAuthenticationToken.cshtml";

    //     return await _emailSender
    //         .SendWithEmbeddedTemplateAsync(to, "Your 2FA Token", model, this.GetType().Assembly, templateName, cancellationToken);
    // }

    // public async Task<bool> SendAccountInformationAsync(string to, AccountInformationModel model, CancellationToken cancellationToken = default)
    // {
    //     var templateName = "AccountInformation.cshtml";

    //     return await _emailSender
    //         .SendWithEmbeddedTemplateAsync(to, "Your Account Information", model, this.GetType().Assembly, templateName, cancellationToken);
    // }

    // public async Task<bool> SendBlockAccountInfomationAsync(string to, BlockAccountModel model, CancellationToken cancellationToken = default)
    // {
    //     var templateName = "BlockAccount.cshtml";

    //     return await _emailSender
    //         .SendWithEmbeddedTemplateAsync(to, "Your Account Has Been Locked", model, this.GetType().Assembly, templateName, cancellationToken);
    // }
}
