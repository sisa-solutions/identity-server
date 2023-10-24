using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class ScopeConfiguration : EntityConfiguration<Scope>
{
    public override void Configure(EntityTypeBuilder<Scope> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(t => t.ConcurrencyToken)
            .HasMaxLength(50)
            .IsConcurrencyToken();

        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .HasDefaultValueSql("''");

        builder.Property(t => t.DisplayName)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder.Property(t => t.DisplayNames)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Descriptions)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Properties)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Resources)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'[]'");
    }
}
