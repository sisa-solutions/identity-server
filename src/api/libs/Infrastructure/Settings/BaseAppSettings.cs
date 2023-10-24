namespace Sisa.Infrastructure.Settings;

public abstract record BaseAppSettings : IAppSettings
{
    public string PathBase { get; init; } = string.Empty;
    public CorsPolicySettings CorsPolicy { get; init; } = new();
    public CertSettings Certs { get; init; } = null!;

    public string AppInstance { get; init; } = string.Empty;
    public string DistributedCacheInstance { get; init; } = string.Empty;

    public Localization Localization { get; init; } = new();
}

public record Localization
{
    public string DefaultCulture { get; init; } = "en";
    public string[] SupportedCultures { get; init; } = new string[] { "en" };
}
