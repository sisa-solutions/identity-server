namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record RegistrationConfirmModel(
    string OrgName,
    string ContactName,
    string FullName,
    string ConfirmEmailUrl
);
