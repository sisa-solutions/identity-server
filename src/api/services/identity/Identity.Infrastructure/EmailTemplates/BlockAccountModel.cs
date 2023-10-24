namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record BlockAccountModel(
    string OrgName,
    string ContactName,
    string ContactEmail,
    string FullName,
    int LockoutDuration
);
