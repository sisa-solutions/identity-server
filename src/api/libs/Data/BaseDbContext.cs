using System.Data;

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using Sisa.Abstractions;

namespace Sisa.Data;

public abstract class BaseDbContext : DbContext, IDataProtectionKeyContext, IUnitOfWork
{
    private ILogger<BaseDbContext> _logger = null!;

    private IDbContextTransaction? _currentTransaction;
    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public void ConfigureLogger<TDbContext>(ILogger<TDbContext> logger) where TDbContext : BaseDbContext
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<bool> SaveEntitiesAsync(bool throwWhenNoRowsWereAffected = true, CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        if (throwWhenNoRowsWereAffected && result <= 0)
            throw new Exception("No rows were effected.");

        return result > 0;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null) return null;


        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current.");

        try
        {
            _logger.LogDebug("Try to commit the transaction ({0}).", transaction.TransactionId);

            await base.SaveChangesAsync(cancellationToken);
            transaction.Commit();
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _logger.LogDebug("Successfully handle the transaction ({0}).", _currentTransaction.TransactionId);

            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null) return;

        try
        {
            _logger.LogDebug("Try to rollback the transaction ({0}).", _currentTransaction.TransactionId);

            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        catch
        {
            _logger.LogError("An error occurred during rollback the transaction ({0}).", _currentTransaction.TransactionId);

            throw;
        }
        finally
        {
            _logger.LogDebug("Successfully rollback the transaction ({0}).", _currentTransaction.TransactionId);

            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
