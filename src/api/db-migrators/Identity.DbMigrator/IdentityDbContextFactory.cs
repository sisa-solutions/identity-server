using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Sisa.Extensions;
using Sisa.Identity.Data;

namespace Sisa.Identity.DbMigrator;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IdentityDbContext> optionsBuilder = new();

        optionsBuilder.UseMigrationDatabase<IdentityDbContextFactory>(nameof(IdentityDbContext));

        return new IdentityDbContext(optionsBuilder.Options);
    }

    /*
     * cd db-migrators/Identity.DbMigrator
     * dotnet ef migrations add Initialize -c Sisa.Identity.Data.IdentityDbContext -o PostgreSQL/Migrations
     *
     * dotnet ef migrations script -i -c Sisa.Identity.Data.IdentityDbContext -o PostgreSQL/Scripts/000_Snapshot.sql
     *
     * dotnet ef migrations script -i -c Sisa.Identity.Data.IdentityDbContext 0 Initialize -o PostgreSQL/Scripts/010_Initialize.sql
     * dotnet ef migrations script -i -c Sisa.Identity.Data.IdentityDbContext Initialize AddPermissionsToRole -o PostgreSQL/Scripts/011_AddPermissionsToRole.sql
     */
}
