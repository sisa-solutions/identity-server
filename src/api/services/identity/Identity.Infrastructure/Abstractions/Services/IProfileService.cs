using System.Security.Claims;

namespace Sisa.Identity.Infrastructure.Abstractions;

public interface IProfileService
{
    // Task<bool> IsActiveAsync(string subject);
    Task<IReadOnlyList<Claim>> GetProfileDataAsync(ClaimsPrincipal principal);
}
