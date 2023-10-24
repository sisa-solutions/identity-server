using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder
            .Property(t => t.ClaimType)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.ClaimValue)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        // Each Role can have many associated RoleClaims
        builder.HasOne(t => t.Role)
            .WithMany(t => t.RoleClaims)
            .HasForeignKey(t => t.RoleId)
            .IsRequired();
    }
}
