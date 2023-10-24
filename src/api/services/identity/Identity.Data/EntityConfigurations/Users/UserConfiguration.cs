using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Enums;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        // Indexes for "normalized" username and email, to allow efficient lookups
        builder.HasIndex(t => t.NormalizedUserName)
            .IsUnique(true);

        builder.HasIndex(t => t.NormalizedEmail);

        builder.HasIndex(t => t.PhoneNumber);

        builder.Property(t => t.ConcurrencyStamp)
            .HasMaxLength(50)
            .IsConcurrencyToken();

        builder.Property(t => t.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Email)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.NormalizedEmail)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.PasswordHash)
            .HasMaxLength(500);

        builder.Property(t => t.SecurityStamp)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.FirstName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.LastName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''");

        builder.Property(t => t.FullName)
            .HasMaxLength(100)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValueSql($"'{Gender.UNSPECIFIED}'");

        builder.Property(t => t.BirthDate)
            .HasColumnType("DATE");

        builder.Property(t => t.Picture)
            .HasMaxLength(500)
            .HasDefaultValueSql("''");

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValueSql($"'{UserStatus.NEW}'");
    }
}
