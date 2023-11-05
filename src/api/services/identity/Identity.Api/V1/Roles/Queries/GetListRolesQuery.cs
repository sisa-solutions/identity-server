using Sisa.Abstractions;

using Sisa.Identity.Api.V1.Roles.Responses;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.Specifications;

namespace Sisa.Identity.Api.V1.Roles.Queries;

public sealed partial class GetListRolesQuery : IQuery<ListRolesResponse>
{
}

public class GetListRolesQueryHandler(
    IRoleRepository repository,
    ILogger<GetListRolesQueryHandler> logger
) : IQueryHandler<GetListRolesQuery, ListRolesResponse>
{
    public async ValueTask<ListRolesResponse> HandleAsync(GetListRolesQuery query, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting categories");

        var spec = new RoleSpecifications<RoleResponse>(
            query.Filter,
            query.SortBy,
            query.Paging,
            RoleProjections.Projection);

        var roles = await repository
            .PaginateAsync(spec, cancellationToken);

        return roles.ToListResponse();
    }
}
