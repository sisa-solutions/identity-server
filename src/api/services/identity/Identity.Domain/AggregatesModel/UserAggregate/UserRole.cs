namespace Sisa.Identity.Domain.AggregatesModel.UserAggregate;

using Microsoft.AspNetCore.Identity;

using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}
