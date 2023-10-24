namespace Sisa.Constants;

public sealed class DatabaseSchemaConstants
{
    private DatabaseSchemaConstants() { }

    public const string MIGRATIONS_TABLE_PRIMARY_KEY = "pk_migrations";
    public const string MIGRATIONS_TABLE = "migrations";
    public const string MIGRATIONS_TABLE_ID = "id";
    public const string MIGRATIONS_TABLE_PRODUCT_VERSION = "version";
    public const string MIGRATIONS_TABLE_CREATED_AT = "created_at";

    public const string DEFAULT_UTC_TIMESTAMP = "timezone('utc', now())";

    public const string DEFAULT_UUID_GENERATOR = "gen_random_uuid()";
}
