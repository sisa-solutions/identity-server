namespace Sisa.Settings;

public abstract record BaseAppSettings : IAppSettings
{
    public string PathBase { get; init; } = string.Empty;
    public CorsPolicySettings CorsPolicy { get; init; } = new();
    public CertSettings Certs { get; init; } = null!;

    public string AppInstance { get; init; } = string.Empty;
    public string DistributedCacheInstance { get; init; } = string.Empty;

    public LocalizationSettings Localization { get; init; } = new();
}
