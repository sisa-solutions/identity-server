namespace Sisa.Settings;

public record class LocalizationSettings
{
    public string DefaultCulture { get; init; } = "en";
    public string[] SupportedCultures { get; init; } = ["en"];
}
