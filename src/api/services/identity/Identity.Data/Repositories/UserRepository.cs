using Microsoft.EntityFrameworkCore;

using Sisa.Abstractions;

using Sisa.Data.Repositories;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Data.Repositories;

[TransientService]
public class UserRepository(IdentityDbContext dbContext) : Repository<IdentityDbContext, User>(dbContext), IUserRepository
{
    private readonly IdentityDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<bool> IsUserExistsAsync(string userName, string email, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users.AsNoTracking();

        query = query.Where(u => u.UserName == userName
            || (!string.IsNullOrEmpty(email) && u.Email == email)
            || (!string.IsNullOrEmpty(phoneNumber) && u.PhoneNumber == phoneNumber));

        return await query.AnyAsync(cancellationToken);
    }
}
