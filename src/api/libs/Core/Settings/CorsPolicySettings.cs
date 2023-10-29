namespace Sisa.Settings;

public record class CorsPolicySettings
{
    public string Origins { get; init; } = default!;

    public string[] GetOrigins()
    {
        return Origins.Split(",");
    }
}
