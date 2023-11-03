using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class ApplicationConfiguration : EntityConfiguration<Application>
{
    public override void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasIndex(x => x.ClientId)
            .IsUnique();

        builder.Property(t => t.ApplicationType)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.ClientType)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.ClientId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.ClientSecret)
            .HasMaxLength(500)
            .HasDefaultValueSql("''");

        builder.Property(t => t.ConcurrencyToken)
            .HasMaxLength(50)
            .IsConcurrencyToken();

        builder.Property(t => t.ConsentType)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.DisplayName)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder.Property(t => t.DisplayNames)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Requirements)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'[]'");

        builder.Property(t => t.Permissions)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'[]'");

        builder.Property(t => t.Properties)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Settings)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.JsonWebKeySet)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.RedirectUris)
            .HasDefaultValueSql("'[]'");

        builder.Property(t => t.PostLogoutRedirectUris)
            .HasDefaultValueSql("'[]'");
    }
}
