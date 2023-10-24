using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Sisa.Abstractions;

using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Infrastructure.Abstractions;

namespace Sisa.Identity.Infrastructure.Handlers;

[TransientService]
public class ResourceOwnerPasswordGrantTypeHandler : IResourceOwnerPasswordGrantTypeHandler
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IProfileService _profileService;
    private readonly ILogger<ResourceOwnerPasswordGrantTypeHandler> _logger;

    public ResourceOwnerPasswordGrantTypeHandler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IProfileService profileService,
        ILogger<ResourceOwnerPasswordGrantTypeHandler> logger
    )
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(signInManager);
        ArgumentNullException.ThrowIfNull(profileService);
        ArgumentNullException.ThrowIfNull(logger);

        _userManager = userManager;
        _signInManager = signInManager;
        _profileService = profileService;
        _logger = logger;
    }

    public async Task<GrantTypeHandleResult> Handle(AuthorizeRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
        {
            _logger.LogError("Invalid credentials");
            throw new Exception("Invalid credentials");

            // return new GrantTypeHandleResult(new DomainException(400, "INVALID_GRANT", "Invalid credentials", "Invalid credentials"));
        }

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null)
        {
            _logger.LogError("Invalid credentials");

            throw new Exception("Invalid credentials");

            // return new GrantTypeHandleResult(new DomainException(404, "INVALID_GRANT", "Invalid credentials", "Invalid credentials"));
        }

        var identityResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        // if (identityResult.Succeeded)
            return new GrantTypeHandleResult(user.Id.ToString());

        // if (identityResult.RequiresTwoFactor)
        //     return new GrantTypeHandleResult(new DomainException(400, "REQUIRE_TWO_FACTOR", "Require Two Factor", "Require Two Factor"));

        // if (identityResult.IsLockedOut)
        //     return new GrantTypeHandleResult(new DomainException(400, "LOCKED_OUT", "Locked out", "Locked out"));

        // return new GrantTypeHandleResult(new DomainException(400, "INVALID_GRANT", "Invalid credentials", "Invalid credentials"));
    }
}
