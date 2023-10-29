using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

using Sisa.Abstractions;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Api.V1.Connect.Commands;

/// <summary>
/// Command to logout a user
/// </summary>
public record LogoutCommand : ICommand<IResult>
{

}

/// <summary>
/// Handler for <see cref="LogoutCommand"/>
/// </summary>
public class LogoutCommandHandler(
    SignInManager<User> signInManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthorizeCommandHandler> logger
) : ICommandHandler<LogoutCommand, IResult>
{
    /// <summary>
    /// Identity service
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<IResult> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Logout command received");

        var httpContext = httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("The HTTP context cannot be retrieved.");

        OpenIddictRequest request = httpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        await signInManager.SignOutAsync();

        return TypedResults.SignOut(
           properties: new AuthenticationProperties
           {
               RedirectUri = request.PostLogoutRedirectUri ?? "/"
           },
           authenticationSchemes: [OpenIddictServerAspNetCoreDefaults.AuthenticationScheme]
       );
    }
}
