namespace Sisa.Settings;

public record class CertSettings
{
    public string DataProtection { get; init; } = string.Empty;
    public string EncryptingSigning { get; init; } = string.Empty;
}
