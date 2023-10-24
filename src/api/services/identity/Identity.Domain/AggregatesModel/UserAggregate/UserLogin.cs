namespace Sisa.Identity.Domain.AggregatesModel.UserAggregate;

using Microsoft.AspNetCore.Identity;

public class UserLogin : IdentityUserLogin<Guid>
{
    public virtual User User { get; set; } = null!;
}
