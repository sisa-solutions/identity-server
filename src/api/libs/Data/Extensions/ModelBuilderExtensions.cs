using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Sisa.Extensions;

public static partial class ModelBuilderExtensions
{
    public static void ApplySnakeCaseConventions(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ToSnakeCase());
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName()?.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
            }
        }
    }

    public static void UseCustomDbFunctions(this ModelBuilder builder)
    {
        builder
            .HasDbFunction(() => StringExtensions.Like(default!, default!))
            .HasTranslation(args => new LikeExpression(
                args[0],
                args[1],
                null,
                null
            ));
    }
}
