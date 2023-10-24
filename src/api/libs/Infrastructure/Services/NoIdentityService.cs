// using Sisa.Enums;

// namespace Sisa.Abstractions.Services;

// public class NoIdentityService : IIdentityService
// {
//     public Guid? TenantId => null;
//     public Guid UserId => Guid.Empty;

//     public string UserName => string.Empty;

//     public string FirstName => string.Empty;

//     public string LastName => string.Empty;

//     public string FullName => string.Empty;

//     public string DisplayName => string.Empty;


//     public IReadOnlyCollection<string> Roles => new string[] { };

//     public IReadOnlyCollection<string> Permissions => new string[] { };

//     public string Email => string.Empty;

//     public bool EmailConfirmed => false;

//     public string PhoneNumber => string.Empty;

//     public bool PhoneNumberConfirmed => false;

//     public DateOnly? BirthDate => null;
//     public Gender Gender => Gender.OTHER;

//     public string GetAccessToken() => string.Empty;

//     public bool TryParseClaim(string type, out string value)
//     {
//         value = string.Empty;

//         return false;
//     }

//     public bool HasClaim(string type, string value) => false;

//     public bool IsInRole(string role) => false;
//     public bool HasPermission(string permission) => false;
//     public TService? GetService<TService>() => default(TService);
// }
