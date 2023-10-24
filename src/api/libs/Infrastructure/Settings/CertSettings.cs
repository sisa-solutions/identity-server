namespace Sisa.Infrastructure.Settings;

public record CertSettings
{
    public string DataProtection { get; init; } = string.Empty;
    public string EncryptingSigning { get; init; } = string.Empty;
}
