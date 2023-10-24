using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sisa.Extensions;

public static class DbMigratorExtensions
{
    public static async Task<IHost> MigrateDbContext<TDbContext>(
        this IHost host,
        Func<IServiceProvider, TDbContext, ILogger<TDbContext>, Task>? seeder = null)
        where TDbContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<TDbContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<TDbContext>>();

            if (dbContext == null)
                throw new NullReferenceException(nameof(dbContext));
            if (logger == null)
                throw new NullReferenceException(nameof(logger));

            try
            {
                await InternalMigrateDbContext(dbContext, logger);

                if (seeder != null)
                {
                    logger.LogInformation("Begin seeding data to {DbContextName}", typeof(TDbContext).Name);

                    await seeder.Invoke(serviceProvider, dbContext, logger);

                    logger.LogInformation("Seeding data to {DbContextName} completed", typeof(TDbContext).Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database used on context {DbContextName}", typeof(TDbContext).Name);

                throw;
            }
        }

        return host;
    }

    private static async Task InternalMigrateDbContext<TDbContext>(
        TDbContext dbContext,
        ILogger<TDbContext> logger)
        where TDbContext : DbContext
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        logger.LogInformation("Found {NoOfPendingMigration} pending migrations associated with context {DbContextName}", pendingMigrations.Count(), typeof(TDbContext).Name);

        if (!pendingMigrations.Any())
            return;

        logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TDbContext).Name);

        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TDbContext).Name);
    }
}
