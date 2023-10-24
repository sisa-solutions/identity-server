namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record TwoFactorAuthenticationModel(
    string OrgName,
    string ContactName,
    string ContactEmail,
    string FullName,
    string Token
);
