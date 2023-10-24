using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

using OpenIddict.EntityFrameworkCore.Models;

using Sisa.Abstractions;

using OpenIddictConstants = OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

public class Application : OpenIddictEntityFrameworkCoreApplication<Guid, Authorization, Token>, IEntity<Guid>
{
    // public string? ClientUri { get; private set; }
    // public string? LogoUri { get; private set; }

    [NotMapped]
    public bool RequireConsent => ConsentType == OpenIddictConstants.ConsentTypes.Explicit;

    [NotMapped]
    public bool RequirePkce { get; set; }

    [NotMapped]
    public bool AllowOfflineAccess { get; set; }

    private readonly HashSet<string> _grantTypes;
    [NotMapped]
    public IReadOnlyCollection<string> GrantTypes => _grantTypes;

    private readonly HashSet<string> _responseTypes;
    [NotMapped]
    public IReadOnlyCollection<string> ResponseTypes => _responseTypes;

    private readonly HashSet<string> _endpoints;
    [NotMapped]
    public IReadOnlyCollection<string> Endpoints => _endpoints;

    private readonly HashSet<string> _identityResources;
    [NotMapped]
    public IReadOnlyCollection<string> IdentityResources => _identityResources;

    private readonly HashSet<string> _scopes;
    [NotMapped]
    public IReadOnlyCollection<string> Scopes => _scopes;

    public Application() : base()
    {
        _grantTypes = [];
        _responseTypes = [];
        _endpoints = [];
        _identityResources = [];
        _scopes = [];
    }

    public Application(string clientId, string? clientSecret) : this()
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    public IReadOnlyList<string> AllowedIdentityResources = new List<string>() { "openid", "email", "phone", "profile", "address" };

    public void MakeWebClient() => ApplicationType = OpenIddictConstants.ApplicationTypes.Web;
    public bool IsWebClient() => ApplicationType == OpenIddictConstants.ApplicationTypes.Web;
    public void MakeNativeClient() => ApplicationType = OpenIddictConstants.ApplicationTypes.Native;
    public bool IsNativeClient() => ApplicationType == OpenIddictConstants.ApplicationTypes.Native;

    public void MakePublic() => ClientType = OpenIddictConstants.ClientTypes.Public;
    public void MakeConfidential() => ClientType = OpenIddictConstants.ClientTypes.Confidential;
    public bool IsPublic() => ClientType == OpenIddictConstants.ClientTypes.Public;
    public bool IsConfidential() => ClientType == OpenIddictConstants.ClientTypes.Confidential;

    public void MakeRequireConsent() => ConsentType = OpenIddictConstants.ConsentTypes.Explicit;
    public void MakeNonRequireConsent() => ConsentType = OpenIddictConstants.ConsentTypes.Implicit;

    public void MakeRequiredPkce()
        => RequirePkce = true;

    public void MakeNoneRequiredPkce()
        => RequirePkce = false;

    public void MakeAllowOfflineAccess()
        => AllowOfflineAccess = true;

    public void MakeDisallowOfflineAccess()
        => AllowOfflineAccess = false;

    // public void SetRedirectUris(IEnumerable<string> uris)
    //     => RedirectUris = JsonSerializer.Serialize(uris, JsonConstants.JSON_SERIALIZER_OPTIONS);

    // public void SetPostLogoutRedirectUris(IEnumerable<string> uris)
    //     => PostLogoutRedirectUris = JsonSerializer.Serialize(uris, JsonConstants.JSON_SERIALIZER_OPTIONS);

    public void SetEndpoints(IEnumerable<string> endpoints)
    {
        _endpoints.Clear();
        _endpoints.UnionWith(endpoints);
    }

    public void SetGrantTypes(IEnumerable<string> grantTypes)
    {
        _grantTypes.Clear();
        _grantTypes.UnionWith(grantTypes);
    }

    public void SetResponseTypes(IEnumerable<string> responseTypes)
    {
        _responseTypes.Clear();
        _responseTypes.UnionWith(responseTypes);
    }

    public void SetScopes(IEnumerable<string> scopes)
    {
        _scopes.Clear();
        _scopes.UnionWith(scopes);
    }

    // public void SetClientInfo(string clientUri, string logoUri)
    // {
    //     ClientUri = clientUri;
    //     LogoUri = logoUri;
    // }

    // public void Load()
    // {
    //     var requirements = JsonSerializer.Deserialize<string[]>(Requirements ?? "[]", JsonConstants.JSON_SERIALIZER_OPTIONS);
    //     var permissions = JsonSerializer.Deserialize<string[]>(Permissions ?? "[]", JsonConstants.JSON_SERIALIZER_OPTIONS);

    //     RequirePkce = requirements?
    //         .Any(x => x == OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange) ?? false;

    //     AllowOfflineAccess = permissions?
    //         .Any(s => s == $"{OpenIddictConstants.Permissions.Prefixes.Scope}{OpenIddictConstants.Scopes.OfflineAccess}") ?? false;

    //     _grantTypes = permissions?
    //         .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType))
    //         .Select(x => x.Replace(OpenIddictConstants.Permissions.Prefixes.GrantType, string.Empty))
    //         .ToHashSet()!;

    //     _responseTypes = permissions?
    //         .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.ResponseType))
    //         .Select(x => x.Replace(OpenIddictConstants.Permissions.Prefixes.ResponseType, string.Empty))
    //         .ToHashSet()!;

    //     var allScopes = permissions?
    //         .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope))
    //         .Select(x => x.Replace(OpenIddictConstants.Permissions.Prefixes.Scope, string.Empty));

    //     var scopes = allScopes?
    //         .Where(f => !string.IsNullOrEmpty(f) &&
    //             f != OpenIddictConstants.Scopes.OfflineAccess &&
    //             f != OpenIddictConstants.Scopes.Roles);

    //     _identityResources = scopes?
    //                 .Where(s => AllowedIdentityResources?.Contains(s) == true)
    //                 .ToHashSet()!;

    //     _scopes = scopes?.Except(IdentityResources ?? new string[] { }).ToHashSet()!;
    // }

    // public void Build()
    // {
    //     HashSet<string> requirements = new();
    //     HashSet<string> permissions = new();

    //     if (RequirePkce)
    //         requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);

    //     if (AllowOfflineAccess)
    //         permissions.Add($"{OpenIddictConstants.Permissions.Prefixes.Scope}{OpenIddictConstants.Scopes.OfflineAccess}");

    //     if (GrantTypes.Any())
    //         permissions.Union(GrantTypes);

    //     if (ResponseTypes.Any())
    //         permissions.Union(ResponseTypes);

    //     if (IdentityResources.Any())
    //         permissions.Union(IdentityResources);

    //     if (Scopes.Any())
    //         permissions.Union(Scopes);

    //     Requirements = JsonSerializer.Serialize(requirements, JsonConstants.JSON_SERIALIZER_OPTIONS);
    //     Permissions = JsonSerializer.Serialize(permissions, JsonConstants.JSON_SERIALIZER_OPTIONS);
    // }
}
