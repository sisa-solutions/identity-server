using Microsoft.AspNetCore;

using OpenIddict.Abstractions;
using OpenIddict.Core;

using Sisa.Abstractions;
using Sisa.Extensions;
using Sisa.Identity.Api.V1.Connect.Commands;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

namespace Sisa.Identity.Api.V1.Connect.Queries;

/// <summary>
/// Represents the query to get the consent.
/// </summary>
public record GetConsentInfoQuery : IQuery<IResult>
{

}

/// <summary>
/// Represents the handler for the <see cref="GetConsentInfoQuery"/>.
/// </summary>
public class GetConsentInfoQueryHandler(
    OpenIddictApplicationManager<Application> applicationManager,
    OpenIddictScopeManager<Scope> scopeManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthorizeCommandHandler> logger
) : IQueryHandler<GetConsentInfoQuery, IResult>
{
    /// <summary>
    /// Handles the <see cref="GetConsentInfoQuery"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(GetConsentInfoQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting consent info.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        Application application = await applicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty, cancellationToken)
            ?? throw new InvalidOperationException("The application details cannot be retrieved.");

        var scopeNames = request.GetScopes();

        var _scopes = await scopeManager
            .ListAsync(scopes =>
                scopes.Where(scope => scopeNames.Contains(scope.Name!))
                    .Select(x => new
                    {
                        x.Name,
                        x.DisplayName,
                        x.Resources,
                    }), cancellationToken)
            .ToListAsync();

        var scopes = _scopes.Select(x => new
        {
            x.Name,
            x.DisplayName,
            // Resources = JsonSerializer.Deserialize<List<string>?>(x.Resources ?? "", JsonConstants.JSON_SERIALIZER_OPTIONS)
        });

        // var resourceNames = scopes.SelectMany(x => x.Resources ?? new List<string>());

        // var resources = await applicationManager
        //     .ListAsync(apps =>
        //         apps.Where(app => resourceNames.Contains(app.ClientId!))
        //             .Select(x => new
        //             {
        //                 Name = x.ClientId,
        //                 x.DisplayName,
        //             }
        //     ), cancellationToken).ToListAsync();

        var response = new
        {
            application.ClientId,
            ClientName = application.DisplayName,
            request.GrantType,
            Scopes = scopes,
            // Resources = resources
        };

        return TypedResults.Ok(response);
    }
}