using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

using Sisa.Abstractions;
using Sisa.Constants;

namespace Sisa.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor = null!;

    private IdentityService() { }

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Guid? TenantId
    {
        get
        {
            return Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.TenantId)?.Value, out var tenantId) ? tenantId : null;
        }
    }

    public Guid UserId
    {
        get
        {
            return Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.Subject)?.Value, out var userId) ? userId : Guid.Empty;
        }
    }

    public string UserName
    {
        get
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;

            return username;
        }
    }

    public string FirstName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.GivenName)?.Value ?? string.Empty;
        }
    }

    public string LastName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.FamilyName)?.Value ?? string.Empty;
        }
    }

    public string FullName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.Name)?.Value ?? string.Empty;
        }
    }

    public string DisplayName
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.DisplayName)?.Value ?? string.Empty;
        }
    }

    public IReadOnlyCollection<string> Roles
    {

        get
        {
            return (_httpContextAccessor.HttpContext?.User?
                .FindAll(SecurityClaimTypes.Role)?
                .Select(t => t.Value) ?? Array.Empty<string>()).ToList();
        }
    }

    public IReadOnlyCollection<string> Permissions
    {

        get
        {
            return (_httpContextAccessor.HttpContext?.User?
                .FindAll(SecurityClaimTypes.Permission)?
                .Select(t => t.Value) ?? Array.Empty<string>()).ToList();
        }
    }

    public string Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.Email)?.Value ?? string.Empty;
        }
    }

    public bool EmailConfirmed
    {
        get
        {
            return bool.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.EmailVerified)?.Value, out var _emailConfirmed) ? _emailConfirmed : false;
        }
    }

    public string PhoneNumber
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.PhoneNumber)?.Value ?? string.Empty;
        }
    }

    public bool PhoneNumberConfirmed
    {
        get
        {
            return bool.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.PhoneNumberVerified)?.Value, out var _phoneNumberVerified) ? _phoneNumberVerified : false;
        }
    }

    public DateOnly? BirthDate
    {
        get
        {
            return DateOnly.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(SecurityClaimTypes.BirthDate)?.Value, out var _birthDate) ? _birthDate : null;
        }
    }

    public string AccessToken
    {
        get
        {
            return (_httpContextAccessor?.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString() ?? string.Empty).Replace("Bearer ", string.Empty);
        }
    }

    public bool TryParseClaim(string type, out string value)
    {
        value = string.Empty;

        if (string.IsNullOrEmpty(type))
            return false;

        if (_httpContextAccessor == null || _httpContextAccessor?.HttpContext == null)
            return false;

        value = _httpContextAccessor.HttpContext.User.FindFirst(type)?.Value ?? string.Empty;

        return !string.IsNullOrEmpty(value);
    }

    public bool HasClaim(string type, string value)
    {
        if (string.IsNullOrEmpty(type))
            return false;

        if (_httpContextAccessor == null || _httpContextAccessor?.HttpContext == null)
            return false;

        return _httpContextAccessor.HttpContext.User.HasClaim(type, value);
    }

    public bool IsInRole(string role)
    {
        if (string.IsNullOrEmpty(role))
            return false;

        if (_httpContextAccessor == null || _httpContextAccessor?.HttpContext == null)
            return false;

        return _httpContextAccessor.HttpContext.User.IsInRole(role);
    }

    public bool HasPermission(string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return false;

        if (_httpContextAccessor == null || _httpContextAccessor?.HttpContext == null)
            return false;

        return _httpContextAccessor.HttpContext.User.HasClaim(c => c.Type == SecurityClaimTypes.Permission && c.Value == permission);
    }

    public TService? GetService<TService>()
    {
        if (_httpContextAccessor == null || _httpContextAccessor?.HttpContext == null)
            return default(TService);

        return _httpContextAccessor.HttpContext.RequestServices.GetService<TService>();
    }
}
