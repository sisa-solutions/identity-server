using Microsoft.EntityFrameworkCore;

using Sisa.Abstractions;
using Sisa.Data.Repositories;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

namespace Sisa.Identity.Data.Repositories;

[TransientService]
public class RoleRepository(
    IdentityDbContext dbContext
    ) : Repository<IdentityDbContext, Role>(dbContext), IRoleRepository
{
    private readonly IdentityDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async ValueTask<Role?> FindRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles
            .Include(x => x.RoleClaims)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return role;
    }
}
