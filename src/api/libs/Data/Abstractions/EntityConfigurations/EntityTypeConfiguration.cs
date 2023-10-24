using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Abstractions;
using Sisa.Domain.AggregatesModel.AuditableAggregate;

namespace Sisa.Data.EntityConfigurations;

public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        Type[] interfaces = builder.Metadata.ClrType.GetInterfaces();

        if (interfaces.Any(p => p == typeof(IDomainEventEntity)))
            builder.Ignore(nameof(IDomainEventEntity.DomainEvents));

        if (interfaces.Any(p => p == typeof(IDeletionAuditing)))
            builder.HasQueryFilter(p => ((IDeletionAuditing)p).DeletedBy == null);

        // if (interfaces.Any(p => p == typeof(IDeletionAuditing)))
        //     builder.HasQueryFilter(t => EF.Property<DateTimeOffset?>(t, nameof(IDeletionAuditing.DeletedAt)) == null);
    }
}

