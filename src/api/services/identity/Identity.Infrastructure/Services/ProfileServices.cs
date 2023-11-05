using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sisa.Abstractions;

using Sisa.Constants;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;

namespace Sisa.Identity.Infrastructure.Services;

[TransientService]
public class ProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<ProfileService> logger
    )
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(roleManager);
        ArgumentNullException.ThrowIfNull(logger);

        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    // public async Task<bool> IsActiveAsync(string subject)
    // {
    //     User? user = await FindUserAsync(principal);

    //     if (user == null)
    //     {
    //         _logger.LogError("User not found.");

    //         return false;
    //     }

    //     var isActive = user.Status != UserStatus.LOCKED_OUT;

    //     return isActive;
    // }

    public async Task<IReadOnlyList<Claim>> GetProfileDataAsync(ClaimsPrincipal principal)
    {
        User? user = await FindUserAsync(principal);

        if (user == null)
        {
            _logger.LogError("User not found.");

            throw new NullReferenceException(nameof(user));
        }

        IEnumerable<Claim> personalUserClaims = GetPersonalClaims(user);
        IEnumerable<Claim> userClaims = await GetUserClaims(user);
        IEnumerable<Claim> roleClaims = await GetRoleClaims(user);
        IEnumerable<Claim> permissionClaims = await GetPermissionClaims(user);

        return personalUserClaims
            .Union(userClaims)
            .Union(permissionClaims)
            .Union(roleClaims)
            .ToList();
    }

    private async Task<User?> FindUserAsync(ClaimsPrincipal principal)
    {
        Claim? subject = principal.Claims.FirstOrDefault(x => x.Type == "sub");

        if (subject == null)
            throw new NullReferenceException("Invalid subject identifier");

        return await _userManager.FindByIdAsync(subject.Value);
    }

    private IEnumerable<Claim> GetPersonalClaims(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(SecurityClaimTypes.Subject, user.Id.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.UserName))
        {
            claims.Add(new Claim(SecurityClaimTypes.Name, user.UserName));
            claims.Add(new Claim(SecurityClaimTypes.PreferredUserName, user.UserName));
        }

        if (!string.IsNullOrWhiteSpace(user.FirstName))
            claims.Add(new Claim(SecurityClaimTypes.GivenName, user.FirstName));

        if (!string.IsNullOrWhiteSpace(user.LastName))
            claims.Add(new Claim(SecurityClaimTypes.FamilyName, user.LastName));

        if (!string.IsNullOrWhiteSpace(user.FullName))
            claims.Add(new Claim(SecurityClaimTypes.FullName, user.FullName));

        if (user.BirthDate != null)
            claims.Add(new Claim(SecurityClaimTypes.BirthDate, user.BirthDate!.ToString()!));

        claims.Add(new Claim(SecurityClaimTypes.Gender, user.Gender.ToString()));

        if (user.Picture != null)
            claims.Add(new Claim(SecurityClaimTypes.Picture, user.Picture));

        if (_userManager.SupportsUserEmail && !string.IsNullOrWhiteSpace(user.Email))
        {
            claims.AddRange(new[]
            {
                new Claim(SecurityClaimTypes.Email, user.Email),
                new Claim(SecurityClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            });
        }

        if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            claims.AddRange(new[]
            {
                    new Claim(SecurityClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(SecurityClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
        }

        return claims;
    }

    private async Task<IEnumerable<Claim>> GetRoleClaims(User user)
    {
        List<Claim> claims = new();

        if (_userManager.SupportsUserRole)
        {
            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(
                roles.Select(role => new Claim(SecurityClaimTypes.Role, role))
            );
        }

        return claims;
    }

    private async Task<IEnumerable<Claim>> GetPermissionClaims(User user)
    {
        List<string> permissions = new();

        if (_userManager.SupportsUserRole && _roleManager.SupportsRoleClaims)
        {
            var rolePermissions = await _roleManager.Roles.AsNoTracking()
                .Include(x => x.UserRoles.Where(ur => ur.UserId == user.Id))
                .Include(x => x.RoleClaims.Where(x => x.ClaimType == SecurityClaimTypes.Permission && !string.IsNullOrEmpty(x.ClaimValue)))
                .SelectMany(x => x.RoleClaims)
                .Select(x => x.ClaimValue!)
                .ToListAsync();

            if (rolePermissions != null)
                permissions.AddRange(rolePermissions);
        }

        if (_userManager.SupportsUserClaim)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userPermissions = userClaims
                .Where(t => t.Type == SecurityClaimTypes.Permission)
                .Select(x => x.Value);

            permissions.AddRange(userPermissions);
        }

        return permissions
            .Distinct()
            .Select(permission => new Claim(SecurityClaimTypes.Permission, permission));
    }

    public async Task<IEnumerable<Claim>> GetUserClaims(User user)
    {
        if (!_userManager.SupportsUserClaim)
            return new List<Claim>();

        return await _userManager.GetClaimsAsync(user);
    }
}
