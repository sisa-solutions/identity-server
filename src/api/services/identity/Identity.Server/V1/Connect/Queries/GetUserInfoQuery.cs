using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;

using OpenIddict.Abstractions;

using Sisa.Abstractions;
using Sisa.Constants;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Server.V1.Connect.Commands;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Server.V1.Connect.Queries;

/// <summary>
///
/// </summary>
public record GetUserInfoQuery : IQuery<IDictionary<string, object>?>
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}

/// <summary>
/// Represents the handler for the <see cref="GetUserInfoQuery"/>.
/// </summary>
public class GetUserInfoQueryHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthorizeCommandHandler> logger
) : IQueryHandler<GetUserInfoQuery, IDictionary<string, object>?>
{
    /// <summary>
    /// Handles the <see cref="GetUserInfoQuery"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IDictionary<string, object>?> HandleAsync(GetUserInfoQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting user info.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        var user = await userManager.FindByIdAsync(query.Id);

        if (user is null)
        {
            logger.LogError("User not found.");

            return null;
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [Claims.Subject] = query.Id
        };

        if (!string.IsNullOrEmpty(user.UserName))
        {
            claims[Claims.Name] = user.UserName;
            claims[Claims.PreferredUsername] = user.UserName;
        }

        if (httpContext.User.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = user.Email ?? string.Empty;
            claims[Claims.EmailVerified] = user.EmailConfirmed;
        }

        if (httpContext.User.HasScope(Scopes.Phone))
        {
            claims[Claims.PhoneNumber] = user.PhoneNumber ?? string.Empty;
            claims[Claims.PhoneNumberVerified] = user.PhoneNumberConfirmed;
        }

        if (httpContext.User.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await userManager.GetRolesAsync(user);
            // claims[SecurityClaimTypes.Permission] = "";
        }

        if (httpContext.User.HasScope(Scopes.Profile))
        {
            if (!string.IsNullOrEmpty(user.FirstName))
                claims[Claims.GivenName] = user.FirstName;

            if (!string.IsNullOrEmpty(user.LastName))
                claims[Claims.FamilyName] = user.LastName;

            claims[Claims.Gender] = user.Gender;

            if (user.BirthDate.HasValue)
                claims[Claims.Birthdate] = user.BirthDate.Value;

            if (!string.IsNullOrEmpty(user.Picture))
                claims[Claims.Picture] = user.Picture;
        }

        return claims;
    }
}