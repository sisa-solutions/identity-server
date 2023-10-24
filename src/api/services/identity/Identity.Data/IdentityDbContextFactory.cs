using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sisa.Identity.Data;

public class IdentityDbContextFactory(
    IDbContextFactory<IdentityDbContext> pooledFactory,
    ILogger<IdentityDbContext> logger
) : IDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext()
    {
        var context = pooledFactory.CreateDbContext();

        context.ConfigureLogger(logger);

        return context;
    }
}
