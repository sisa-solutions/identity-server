namespace Sisa.Infrastructure.Settings;

public class AwsSettings
{
    public string ServiceUrl { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public long ChunkSize { get; set; } = 5_242_880L;
    public string DefaultBucket { get; set; } = string.Empty;
    public bool DisablePayloadSigning { get; set; } = false;

    public AwsSettings() { }

    public AwsSettings(
        string region,
        string accessKey,
        string secretKey,
        long chunkSize = 5_242_880L,
        bool disablePayloadSigning = false,
        string defaultBucket = ""
    ) : this()
    {
        Region = region;
        AccessKey = accessKey;
        SecretKey = secretKey;
        ChunkSize = chunkSize;
        DisablePayloadSigning = disablePayloadSigning;
        DefaultBucket = defaultBucket;
    }

    public AwsSettings(
        string serviceUrl,
        string region,
        string accessKey,
        string secretKey,
        long chunkSize = 5_242_880L,
        bool disablePayloadSigning = false,
        string defaultBucket = ""
    ) : this(region, accessKey, secretKey, chunkSize, disablePayloadSigning, defaultBucket)
    {
        ServiceUrl = serviceUrl;
    }
}
