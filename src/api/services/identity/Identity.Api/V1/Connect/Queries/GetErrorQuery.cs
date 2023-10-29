using Microsoft.AspNetCore;

using OpenIddict.Abstractions;

using Sisa.Abstractions;
using Sisa.Identity.Api.V1.Connect.Commands;
using Sisa.Identity.Api.V1.Connect.Responses;

namespace Sisa.Identity.Api.V1.Connect.Queries;

/// <summary>
/// Represents the query to get the consent.
/// </summary>
public record GetErrorQuery : IQuery<IResult>
{

}

/// <summary>
/// Represents the handler for the <see cref="GetErrorQuery"/>.
/// </summary>
public class GetErrorQueryHandler(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthorizeCommandHandler> logger
) : IQueryHandler<GetErrorQuery, IResult>
{
    /// <summary>
    /// Handles the <see cref="GetErrorQuery"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(GetErrorQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting consent info.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictResponse? response = httpContext.GetOpenIddictServerResponse();

        return TypedResults.BadRequest(await Task.FromResult(new ErrorResponse()
        {
            Error = response?.Error ?? "unknown_error",
            ErrorDescription = response?.ErrorDescription ?? "An unknown error has occurred."
        }));
    }
}