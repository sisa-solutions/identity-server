namespace Sisa.Identity.Settings;

using Sisa.Settings;

public record AppSettings : BaseAppSettings
{
    public IdentitySettings Identity { get; set; } = new();
    // public OpenApiSettings OpenApi { get; set; } = new();
    public EmailSettings Email { get; set; } = new();
}

public record EmailSettings
{
    public SmtpSettings Smtp { get; set; } = new();
    public EmailSender Sender { get; set; } = new();
    public Contact Contact { get; set; } = new();
}
