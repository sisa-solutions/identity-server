using Sisa.Identity.Infrastructure.EmailTemplates;

namespace Sisa.Identity.Infrastructure.Abstractions;

public interface IEmailService
{
    Task<bool> SendConfirmEmailAsync(
        string to, ConfirmEmailModel model, CancellationToken cancellationToken = default);

    Task<bool> SendResetPasswordAsync(
        string to, ResetPasswordModel model, CancellationToken cancellationToken = default);

    Task<bool> SendTwoFactorAuthenticationTokenAsync(
        string to, TwoFactorAuthenticationModel model, CancellationToken cancellationToken = default);

    Task<bool> SendAccountInformationAsync(
        string to, AccountInformationModel model, CancellationToken cancellationToken = default);

    Task<bool> SendBlockAccountInfomationAsync(
        string to, BlockAccountModel model, CancellationToken cancellationToken = default);
}
