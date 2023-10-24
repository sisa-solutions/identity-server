using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Identity.Domain.PermissionAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class PermissionGroupConfiguration : EntityConfiguration<PermissionGroup>
{
    public override void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");
    }
}
