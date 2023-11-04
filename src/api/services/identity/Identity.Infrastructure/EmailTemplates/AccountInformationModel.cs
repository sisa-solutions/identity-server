namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record AccountInformationModel(
    string Company,
    string ContactName,
    string ContactEmail,
    string FullName,
    string UserName,
    string Password
);
