using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

using Sisa.Constants;

namespace Sisa.Data.Repositories;

#pragma warning disable EF1001 // Internal EF Core API usage.
public class CustomHistoryRepository : NpgsqlHistoryRepository
#pragma warning restore EF1001 // Internal EF Core API usage.
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public CustomHistoryRepository([NotNull] HistoryRepositoryDependencies dependencies) : base(dependencies)
#pragma warning restore EF1001 // Internal EF Core API usage.
    {
    }

    protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
    {
        history.ToTable(DatabaseSchemaConstants.MIGRATIONS_TABLE);

        history.HasKey(h => h.MigrationId)
            .HasName(DatabaseSchemaConstants.MIGRATIONS_TABLE_PRIMARY_KEY);

        history.Property(h => h.MigrationId)
            .HasColumnName(DatabaseSchemaConstants.MIGRATIONS_TABLE_ID)
            .IsRequired()
            .HasMaxLength(100);

        history.Property(h => h.ProductVersion)
            .HasColumnName(DatabaseSchemaConstants.MIGRATIONS_TABLE_PRODUCT_VERSION)
            .IsRequired()
            .HasMaxLength(50);

        history.Property<DateTimeOffset>(DatabaseSchemaConstants.MIGRATIONS_TABLE_CREATED_AT)
            .IsRequired()
            .HasColumnName(DatabaseSchemaConstants.MIGRATIONS_TABLE_CREATED_AT)
            .HasDefaultValueSql(DatabaseSchemaConstants.DEFAULT_UTC_TIMESTAMP);
    }
}
