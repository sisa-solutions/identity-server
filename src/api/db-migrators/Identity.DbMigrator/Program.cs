using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sisa.Constants;
using Sisa.Extensions;
using Sisa.Identity.Data;
using Sisa.Identity.DbMigrator.Seeds;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

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

                services.AddOpenIddict()
                    .AddCore(options =>
                    {
                        options.UseEntityFrameworkCore()
                            .UseDbContext<IdentityDbContext>()
                            .ReplaceDefaultEntities<Application, Authorization, Scope, Token, Guid>();
                    });

                services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddDefaultTokenProviders();
            })
            .Build();

        await host.MigrateDbContext<IdentityDbContext>(Seed_Release_001.SeedAsync);

        Environment.Exit(-1);
    }
}
