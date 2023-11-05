using FluentValidation;

using Microsoft.AspNetCore.Identity;

using Sisa.Abstractions;

using Sisa.Identity.Api.V1.Roles.Responses;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

namespace Sisa.Identity.Api.V1.Roles.Commands;

public sealed partial class CreateRoleCommand : ICommand<RoleResponse>
{
}

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
    }
}

public class CreateRoleCommandHandler(
    RoleManager<Role> roleManager,
    ILogger<CreateRoleCommandHandler> logger
) : ICommandHandler<CreateRoleCommand, RoleResponse>
{
    public async ValueTask<RoleResponse> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating Role with name {name}", command.Name);

        var isRoleExists = await roleManager.RoleExistsAsync(command.Name);

        if (isRoleExists)
        {
            logger.LogWarning("Role already exists");

            throw new DomainException(409, "role_already_exists", "Role already exists");
        }

        Role role = new(command.Name, command.Description);

        if (command.Permissions.Count != 0)
        {
            logger.LogInformation("Creating Role with permissions {permissions}", command.Permissions);

            role.UpdatePermissions(command.Permissions);
        }

        await roleManager.CreateAsync(role);

        return role.ToResponse();
    }
}
