using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;

using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.Server.AspNetCore;

using Sisa.Abstractions;
using Sisa.Identity.Server.V1.Connect.Responses;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.Server.V1.Connect.Queries;

/// <summary>
/// Represents the query to get the verification info.
/// </summary>
public class GetVerificationInfoQuery : IQuery<IResult>
{

}

/// <summary>
/// Represents the handler for the <see cref="GetVerificationInfoQuery"/>.
/// </summary>
/// <param name="applicationManager"></param>
/// <param name="httpContextAccessor"></param>
/// <param name="logger"></param>
public class GetVerificationInfoQueryHandler(
    OpenIddictApplicationManager<Application> applicationManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetVerificationInfoQuery> logger
) : IQueryHandler<GetVerificationInfoQuery, IResult>
{
    /// <summary>
    /// Handles the <see cref="GetVerificationInfoQuery"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async ValueTask<IResult> HandleAsync(GetVerificationInfoQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting verification info.");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // If the user code was not specified in the query string (e.g as part of the verification_uri_complete),
        // render a form to ask the user to enter the user code manually (non-digit chars are automatically ignored).
        if (string.IsNullOrEmpty(request.UserCode))
        {
            return TypedResults.BadRequest(new VerifyResponse());
        }

        // Retrieve the claims principal associated with the user code.
        var result = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        if (result.Succeeded)
        {
            // Retrieve the application details from the database using the client_id stored in the principal.
            var application = await applicationManager.FindByClientIdAsync(result.Principal.GetClaim(Claims.ClientId) ?? string.Empty, cancellationToken) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Render a form asking the user to confirm the authorization demand.
            return TypedResults.Ok(new VerifyResponse
            {
                ApplicationName = await applicationManager.GetLocalizedDisplayNameAsync(application, cancellationToken) ?? string.Empty,
                Scope = string.Join(" ", result.Principal.GetScopes()),
                UserCode = request.UserCode
            });
        }

        // Redisplay the form when the user code is not valid.
        return TypedResults.BadRequest(new VerifyResponse
        {
            Error = Errors.InvalidToken,
            ErrorDescription = "The specified user code is not valid. Please make sure you typed it correctly."
        });
    }
}
