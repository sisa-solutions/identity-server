using Sisa.Abstractions;

using Sisa.Identity.Api.V1.Roles.Responses;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.Specifications;

namespace Sisa.Identity.Api.V1.Roles.Queries;

public sealed partial class GetSingleRoleQuery : IQuery<RoleResponse>
{
}

public class GetSingleRoleQueryHandler(
    IRoleRepository repository,
    ILogger<GetSingleRoleQueryHandler> logger
) : IQueryHandler<GetSingleRoleQuery, RoleResponse>
{
    public async ValueTask<RoleResponse> HandleAsync(GetSingleRoleQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting categories");

        var spec = new RoleSpecifications<RoleResponse>(
            query.Filter,
            RoleProjections.Projection);

        var role = await repository
            .FindAsync(spec, cancellationToken);

        if (role is null)
        {
            logger.LogWarning("Role not found");

            throw new DomainException(404, "role_not_found", "Role not found");
        }

        return role;
    }
}
