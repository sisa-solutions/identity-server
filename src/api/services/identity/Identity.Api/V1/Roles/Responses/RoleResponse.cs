using System.Linq.Expressions;

using Sisa.Abstractions;

using Sisa.Common.Responses;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;


namespace Sisa.Identity.Api.V1.Roles.Responses;

public sealed partial class RoleResponse
{
    public RoleResponse(IEnumerable<string> permissions) : base()
    {
        Permissions.AddRange(permissions);
    }
}

public static partial class RoleProjections
{
    public static ListRolesResponse ToListResponse(this IPaginatedList<RoleResponse> roles)
    {
        var response = new ListRolesResponse()
        {
            Value = { roles },
            Paging = new PagingInfoResponse
            {
                ItemCount = roles.ItemCount,
                PageIndex = roles.PageIndex,
                PageSize = roles.PageSize,
                PageCount = roles.PageCount
            }
        };

        return response;
    }

    public static RoleResponse ToResponse(this Role file)
    {
        return Projection.Compile().Invoke(file);
    }

    public static Expression<Func<Role, RoleResponse>> Projection
    {
        get
        {
            return x => new RoleResponse(x.Permissions)
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description,
                Predefined = x.Predefined,
                Enabled = x.Enabled,
            };
        }
    }
}
