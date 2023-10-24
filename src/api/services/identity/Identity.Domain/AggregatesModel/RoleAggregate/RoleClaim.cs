using Microsoft.AspNetCore.Identity;

namespace Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public virtual Role Role { get; set; } = null!;
}
