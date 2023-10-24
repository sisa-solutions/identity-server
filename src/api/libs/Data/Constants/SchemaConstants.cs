namespace Sisa.Constants;

public sealed partial class SchemaConstants
{
    private SchemaConstants() { }

    public const string IDENTITY_TABLE_PREFIX = "AspNet";
    public const string IDENTITY_API_TABLE_PREFIX = "OpenIddict";

    public const string DEFAULT_CONNECTION = "DefaultConnection";
    public const string DATABASE_CONNECTION = "DatabaseConnection";
    public const string DISTRIBUTED_CACHING_CONNECTION = "DistributedCachingConnection";
    public const string APPLICATION_DB_CONNECTION = "ApplicationDb";
    public const string EXTRA_PROPERTIES = "ExtraProperties";

    public const double COMMAND_TIMEOUT = 30;
    public const int MAX_RETRY_COUNT = 3;
    public const short MAX_STRING_COLUMN_LENGTH = 2000;
}
