using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public interface IUserRepository : IRepository<User>
{
    Task<bool> IsUserExistsAsync(string userName, string email, string phoneNumber,
        CancellationToken cancellationToken = default);
}
