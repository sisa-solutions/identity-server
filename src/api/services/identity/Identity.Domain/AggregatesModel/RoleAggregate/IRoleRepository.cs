using Sisa.Abstractions;

namespace Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public interface IRoleRepository : IRepository<Role>
{
    ValueTask<Role?> FindRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
