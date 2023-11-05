using FluentValidation;

using Microsoft.AspNetCore.Identity;

using Sisa.Abstractions;

using Sisa.Identity.Api.V1.Roles.Responses;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

namespace Sisa.Identity.Api.V1.Roles.Commands;

public sealed partial class UpdateRoleCommand : ICommand<RoleResponse>
{
    public Guid ParsedId => Guid.TryParse(Id, out Guid id) ? id : Guid.Empty;
    public string NormalizedName => Name.ToUpperInvariant();
}

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
    }
}

public class UpdateRoleCommandHandler(
    IRoleRepository roleRepository,
    RoleManager<Role> roleManager,
    ILogger<UpdateRoleCommandHandler> logger
) : ICommandHandler<UpdateRoleCommand, RoleResponse>
{
    public async ValueTask<RoleResponse> HandleAsync(UpdateRoleCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating Role with name {name}", command.Name);

        var isRoleExists = await roleRepository.ExistAsync(x => x.Id != command.ParsedId && x.NormalizedName == command.NormalizedName, cancellationToken);

        if (isRoleExists)
        {
            logger.LogWarning("Role already exists");

            throw new DomainException(409, "role_already_exists", "Role already exists");
        }

        Role? role = await roleManager.FindByIdAsync(command.Id);

        if (role is null)
        {
            logger.LogWarning("Role not found");

            throw new DomainException(404, "role_not_found", "Role not found");
        }

        role.Update(command.Name, command.Description);

        if (command.Enabled)
        {
            role.Enable("");
        }
        else
        {
            role.Disable("");
        }

        role.UpdatePermissions(command.Permissions);

        await roleManager.UpdateAsync(role);

        return role.ToResponse();
    }
}
