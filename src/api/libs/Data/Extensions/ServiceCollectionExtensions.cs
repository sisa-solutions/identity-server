using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Sisa.Abstractions;

namespace Sisa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataProtectionContext<TDbContext>(
        this IServiceCollection services,
        string applicationName,
        int lifetime = 90,
        X509Certificate2[]? certs = null)
        where TDbContext : DbContext, IDataProtectionKeyContext
    {
        var builder = services.AddDataProtection()
            .SetApplicationName(applicationName)
            .SetDefaultKeyLifetime(TimeSpan.FromDays(lifetime))
            .PersistKeysToDbContext<TDbContext>();

        if (certs == null || certs.Length < 2)
            return services;

        builder.ProtectKeysWithCertificate(certs[0])
            .UnprotectKeysWithAnyCertificate(certs[1..]);

        return services;
    }

    public static IServiceCollection AddPooledDbContext<TDbContext, TDbContextFactory>(
        this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> optionsAction, int poolSize = 1024)
        where TDbContext : DbContext, IUnitOfWork
        where TDbContextFactory : class, IDbContextFactory<TDbContext>
    {
        #region DbContext

        // 1. First, register a pooling context factory as a Singleton service, as usual
        services
            .AddPooledDbContextFactory<TDbContext>(optionsAction, poolSize);

        // 2. Register an additional context factory as a Scoped service,
        // which gets a pooled context from the Singleton factory we registered above,
        // finds the required services (e.g. ILogger), and injects them into the context
        services.AddScoped<TDbContextFactory>();

        // 3. Finally, arrange for a context to get injected from our Scoped factory
        services.AddScoped(
            sp => sp.GetRequiredService<TDbContextFactory>().CreateDbContext());

        #endregion

        return services;
    }
}
