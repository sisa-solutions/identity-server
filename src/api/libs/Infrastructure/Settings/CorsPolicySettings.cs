namespace Sisa.Infrastructure.Settings;

public record CorsPolicySettings
{
    public string Origins { get; init; } = default!;

    public string[] GetOrigins()
    {
        return Origins.Split(",");
    }
}
