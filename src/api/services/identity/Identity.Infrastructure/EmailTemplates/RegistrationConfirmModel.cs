namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record RegistrationConfirmModel(
    string Company,
    string UserName,
    string Code,
    string Url
);
