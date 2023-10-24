using Microsoft.EntityFrameworkCore;

using ILikeExpression = Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal.PostgresILikeExpression;

namespace Sisa.Extensions;

public static partial class ModelBuilderExtensions
{
    public static void ConfigureExtensions(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("unaccent");
    }

    public static void UseCustomPostgreSQLDbFunctions(this ModelBuilder builder)
    {
        builder
            .HasDbFunction(() => StringExtensions.ILike(default!, default!))
#pragma warning disable EF1001 // Internal EF Core API usage.
            .HasTranslation(args => new ILikeExpression(
#pragma warning restore EF1001 // Internal EF Core API usage.
                args[0],
                args[1],
                null,
                null
            ));
    }
}
