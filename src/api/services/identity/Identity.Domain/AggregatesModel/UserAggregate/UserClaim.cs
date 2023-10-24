namespace Sisa.Identity.Domain.AggregatesModel.UserAggregate;

using Microsoft.AspNetCore.Identity;

public class UserClaim : IdentityUserClaim<Guid>
{
    public virtual User User { get; private set; } = null!;
}
