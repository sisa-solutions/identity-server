using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sisa.Constants;
using Sisa.Extensions;
using Sisa.Identity.Data;

namespace Sisa.Identity.DbMigrator;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString(SchemaConstants.DEFAULT_CONNECTION)!;

                services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
                {
                    options.UseMigrationDatabase<IdentityDbContext>(connectionString);
                });
            })
            .Build();

        await host.MigrateDbContext<IdentityDbContext>();

        Environment.Exit(-1);
    }
}
