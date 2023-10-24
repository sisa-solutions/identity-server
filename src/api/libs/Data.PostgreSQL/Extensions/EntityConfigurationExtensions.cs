using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sisa.Abstractions;
using Sisa.Constants;

namespace Sisa.Extensions;

public static class EntityConfigurationExtensions
{
    public static void MapId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity<Guid>
    {
        builder.MapId(DatabaseSchemaConstants.DEFAULT_UUID_GENERATOR);
    }

    public static void MapExtraProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity, IExtraProperties
    {
        builder.MapExtraProperties(SchemaConstants.EXTRA_PROPERTIES);
    }
}
