using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Data.EntityConfigurations;
using Sisa.Identity.Domain.AggregatesModel.SecurityEventAggregate;

namespace Sisa.Identity.Data.EntityTypeConfigurations;

public class SecurityEventConfiguration : EntityConfiguration<SecurityEvent>
{
    public override void Configure(EntityTypeBuilder<SecurityEvent> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.Action)
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired(true)
            .HasDefaultValueSql($"'{SecurityAction.OTHER}'");

        builder
            .Property(x => x.Ip)
            .HasMaxLength(50);

        builder
            .Property(x => x.Agent)
            .HasMaxLength(500);

        builder
            .Property(x => x.Culture)
            .HasMaxLength(10);

        builder
            .Property(x => x.Protocol)
            .HasMaxLength(10);

        builder
            .Property(x => x.Schema)
            .HasMaxLength(10);

        builder
            .Property(x => x.Origin)
            .HasMaxLength(50);

        builder
            .Property(x => x.Uri)
            .HasMaxLength(500);

        builder
           .Property(x => x.Method)
           .HasMaxLength(10);

        builder
           .Property(x => x.CorrelationId)
           .HasMaxLength(100);

        builder
           .Property(x => x.ApplicationName)
           .HasMaxLength(100);

        builder
           .Property(x => x.TenantName)
           .HasMaxLength(100);

        builder
           .Property(x => x.UserName)
           .HasMaxLength(50);

        builder
           .Property(x => x.Exception)
           .HasMaxLength(2000);

        builder
           .Property(x => x.OldValues)
           .HasColumnType("jsonb");

        builder
           .Property(x => x.NewValues)
           .HasColumnType("jsonb");

        builder
            .Property(x => x.Remarks)
            .HasMaxLength(500);
    }
}
