namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record ResetPasswordModel(
    string OrgName,
    string ContactName,
    string ContactEmail,
    string FullName,
    string ResetPasswordUrl
);
