using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        // Composite primary key consisting of the LoginProvider and the key to use
        // with that provider
        builder.HasKey(t => new { t.LoginProvider, t.ProviderKey });

        builder
            .Property(t => t.ProviderKey)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.LoginProvider)
            .HasMaxLength(100)
            .HasDefaultValueSql("''");

        builder
            .Property(t => t.ProviderDisplayName)
            .HasMaxLength(200)
            .HasDefaultValueSql("''");

        // Each User can have many UserLogins
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserLogins)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}
