namespace Sisa.Identity.Infrastructure.EmailTemplates;

public record ConfirmEmailModel(
    string OrgName,
    string ContactName,
    string FullName,
    string ConfirmEmailUrl
);
