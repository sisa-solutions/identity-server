namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record AccountInformationModel(
    string OrgName,
    string ContactName,
    string ContactEmail,
    string FullName,
    string UserName,
    string Password
);
