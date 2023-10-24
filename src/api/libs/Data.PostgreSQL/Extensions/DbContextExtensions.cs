using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using Sisa.Abstractions;
using Sisa.Constants;
using Sisa.Data.Interceptors;
using Sisa.Data.Repositories;

namespace Sisa.Extensions;

public static class DbContextExtensions
{
    public static DbContextOptionsBuilder UseMigrationDatabase<TMigrationsAssembly>(this DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(
            connectionString,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE);
                sqlOptions.MigrationsAssembly(typeof(TMigrationsAssembly).Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        return options.ConfigureCommonSettings();
    }

    public static DbContextOptionsBuilder UseMigrationDatabase<TMigrationsAssembly>(this DbContextOptionsBuilder options, string connectionString, string schema)
    {
        options.UseNpgsql(
            connectionString,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE, schema);
                sqlOptions.MigrationsAssembly(typeof(TMigrationsAssembly).Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        return options.ConfigureCommonSettings();
    }

    public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder options, IServiceProvider serviceProvider, string connectionString)
    {
        options.UseNpgsql(
            connectionString,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        options.ConfigureCommonSettings();

        options.AddInterceptors(new SaveChangesInterceptor(
            serviceProvider.GetRequiredService<IIdentityService>(),
            serviceProvider.GetRequiredService<IEventPublisher>())
        );

        return options;
    }

    public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder options, string connectionString, IServiceProvider serviceProvider, string schema)
    {
        options.UseNpgsql(
            connectionString,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE, schema);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        options.ConfigureCommonSettings();

        options.AddInterceptors(new SaveChangesInterceptor(
            serviceProvider.GetRequiredService<IIdentityService>(),
            serviceProvider.GetRequiredService<IEventPublisher>())
        );

        return options;
    }

    public static DbContextOptionsBuilder UseMigrationDatabase<TMigrationsAssembly>(this DbContextOptionsBuilder options, NpgsqlDataSource dataSource)
    {
        options.UseNpgsql(
            dataSource,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE);
                sqlOptions.MigrationsAssembly(typeof(TMigrationsAssembly).Assembly.GetName().Name);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        return options.ConfigureCommonSettings();
    }

    public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder options, IServiceProvider serviceProvider, NpgsqlDataSource dataSource)
    {
        options.UseNpgsql(
            dataSource,
            sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(SchemaConstants.COMMAND_TIMEOUT).TotalSeconds);
                sqlOptions.MigrationsHistoryTable(DatabaseSchemaConstants.MIGRATIONS_TABLE);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: SchemaConstants.MAX_RETRY_COUNT, maxRetryDelay: TimeSpan.FromSeconds(SchemaConstants.COMMAND_TIMEOUT), errorCodesToAdd: null);
            });

        options.ConfigureCommonSettings();

        options.AddInterceptors(new SaveChangesInterceptor(
            serviceProvider.GetRequiredService<IIdentityService>(),
            serviceProvider.GetRequiredService<IEventPublisher>())
        );

        return options;
    }

    private static DbContextOptionsBuilder ConfigureCommonSettings(this DbContextOptionsBuilder options)
    {
        // options.UseSnakeCaseNamingConvention();

#if DEBUG
        options.EnableSensitiveDataLogging();
#endif

        options.ReplaceService<IHistoryRepository, CustomHistoryRepository>();

        return options;
    }
}
