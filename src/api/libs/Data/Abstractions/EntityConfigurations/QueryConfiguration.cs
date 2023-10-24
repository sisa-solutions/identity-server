using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Abstractions;

namespace Sisa.Data.EntityConfigurations;

public abstract class QueryConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IQueryEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasNoKey()
            .Metadata.SetIsTableExcludedFromMigrations(true);
    }
}
