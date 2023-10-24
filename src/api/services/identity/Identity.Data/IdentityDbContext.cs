using Microsoft.EntityFrameworkCore;

using Sisa.Data;
using Sisa.Extensions;

namespace Sisa.Identity.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : BaseDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureExtensions();
        modelBuilder.UseCustomDbFunctions();
        modelBuilder.UseCustomPostgreSQLDbFunctions();



        modelBuilder.ApplySnakeCaseConventions();
    }
}
