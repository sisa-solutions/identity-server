using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        // Composite primary key consisting of the UserId, LoginProvider and Name
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        builder
            .Property(t => t.Name)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.LoginProvider)
            .HasMaxLength(100)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.Value)
            .HasMaxLength(500)
            .HasDefaultValueSql("''");

        // Each User can have many UserTokens
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserTokens)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}
