using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder
            .Property(t => t.ClaimType)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.ClaimValue)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        // Each User can have many UserClaims
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserClaims)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}
