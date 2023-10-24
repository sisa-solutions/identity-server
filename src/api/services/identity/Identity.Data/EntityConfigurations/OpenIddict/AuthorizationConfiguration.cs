using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Extensions;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class AuthorizationConfiguration : EntityConfiguration<Authorization>
{
    public override void Configure(EntityTypeBuilder<Authorization> builder)
    {
        base.Configure(builder);

        builder.MapId();

        builder.HasIndex(
            $"{nameof(Authorization.Application)}{nameof(Application.Id)}",
            nameof(Authorization.Status),
            nameof(Authorization.Subject),
            nameof(Authorization.Type));

        builder.Property(t => t.ConcurrencyToken)
            .HasMaxLength(50)
            .IsConcurrencyToken();

        builder.Property(t => t.CreationDate)
            .HasColumnName("created_at");

        builder.Property(t => t.Properties)
            .HasDefaultValueSql("'{}'");

        builder.Property(t => t.Scopes)
            .HasMaxLength(2000)
            .HasDefaultValueSql("'[]'");

        builder.Property(t => t.Status)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Subject)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Type)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.HasOne(x => x.Application)
            .WithMany(x => x.Authorizations)
            .HasForeignKey($"{nameof(Application)}{nameof(Application.Id)}");
    }
}
