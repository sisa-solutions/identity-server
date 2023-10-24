using Microsoft.AspNetCore.Identity;

using Sisa.Abstractions;

using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Infrastructure.Abstractions;

[TransientService]
public class EndSessionHandler : IEndSessionHandler
{
    private readonly SignInManager<User> _signInManager;

    public EndSessionHandler(SignInManager<User> signInManager)
    {
        ArgumentNullException.ThrowIfNull(signInManager);

        _signInManager = signInManager;
    }

    public async Task Handle()
    {
        // Ask ASP.NET Core Identity to delete the local and external cookies created
        // when the user agent is redirected from the external identity provider
        // after a successful authentication flow (e.g Google or Facebook).

        await _signInManager.SignOutAsync();
    }
}
