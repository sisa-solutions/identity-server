namespace Sisa.Identity.Settings;

using Sisa.Settings;

public record AppSettings : BaseAppSettings
{
    public IdentitySettings Identity { get; set; } = new();
    // public OpenApiSettings OpenApi { get; set; } = new();
    // public EmailSenderSettings Email { get; set; } = new();
}


// public record EmailSenderSettings : BaseEmailSenderSettings
// {
//     public string OrgName { get; set; } = string.Empty;
//     public Contact Contact { get; set; } = new();
// }
