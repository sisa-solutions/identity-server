namespace Sisa.Abstractions;

public interface IIdentityService
{
    Guid? TenantId { get; }

    Guid UserId { get; }
    string UserName { get; }

    string Email { get; }
    bool EmailConfirmed { get; }

    string PhoneNumber { get; }
    bool PhoneNumberConfirmed { get; }

    IReadOnlyCollection<string> Roles { get; }
    IReadOnlyCollection<string> Permissions { get; }

    string AccessToken { get; }

    bool TryParseClaim(string type, out string value);
    bool HasClaim(string type, string value);

    bool IsInRole(string role);
    bool HasPermission(string permission);
    TService? GetService<TService>();
}
