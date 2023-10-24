namespace Sisa.Infrastructure.Settings;

public interface IAppSettings
{
    string PathBase { get; }
    CorsPolicySettings CorsPolicy { get; }
    CertSettings Certs { get; }

    string AppInstance { get; }
    string DistributedCacheInstance { get; }
}
