using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Abstractions;

namespace Sisa.Extensions;

public static class EntityConfigurationExtensions
{
    public static void MapId<TEntity>(this EntityTypeBuilder<TEntity> builder, string defaultValue)
        where TEntity : class, IEntity<Guid>
    {
        builder.Property(t => t.Id)
            .HasDefaultValueSql(defaultValue);
    }

    public static void MapExtraProperties<TEntity>(this EntityTypeBuilder<TEntity> builder, string columnType)
        where TEntity : class, IEntity, IExtraProperties
    {
        builder.Property("ExtraProperties")
            .HasColumnType(columnType);
    }
}
