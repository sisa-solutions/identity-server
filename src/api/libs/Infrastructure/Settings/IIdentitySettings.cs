namespace Sisa.Infrastructure.Settings;

public interface IIdentitySettings
{
    string Authority { get; }
    string[] Audiences { get; }
    string ClientId { get; }
    string ClientSecret { get; }
}
