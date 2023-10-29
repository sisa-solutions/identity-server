using System.Security.Claims;

using OpenIddict.Abstractions;

using Sisa.Constants;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Infrastructure.Helpers;

/// <inheritdoc/>
public static class OpenIddictHelper
{
    /// <inheritdoc/>
    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case SecurityClaimTypes.Name:
                yield return Destinations.AccessToken;

                if (claim.Subject?.HasScope(Scopes.Profile) == true)
                    yield return Destinations.IdentityToken;

                yield break;

            case SecurityClaimTypes.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject?.HasScope(Scopes.Email) == true)
                    yield return Destinations.IdentityToken;

                yield break;

            case SecurityClaimTypes.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject?.HasScope(Scopes.Roles) == true)
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp":

                yield break;

            default:
                yield return Destinations.AccessToken;

                yield break;
        }
    }
}
