using Grpc.Core;

using Sisa.Abstractions;
using Sisa.Extensions;

using Sisa.Identity.Api.V1.Roles.Commands;
using Sisa.Identity.Api.V1.Roles.Queries;
using Sisa.Identity.Api.V1.Roles.Responses;

namespace Sisa.Identity.Api.V1.Roles;

[GrpcService]
public sealed class RoleService(IMediator mediator) : RoleGrpcService.RoleGrpcServiceBase
{
    public override async Task<ListRolesResponse> GetListRoles(GetListRolesQuery request, ServerCallContext context)
        => await mediator.SendAsync(request, context.CancellationToken).ConfigureAwait(false);

    public override async Task<RoleResponse> GetSingleRole(GetSingleRoleQuery request, ServerCallContext context)
        => await mediator.SendAsync(request, context.CancellationToken).ConfigureAwait(false);

    public override async Task<RoleResponse> CreateRole(CreateRoleCommand request, ServerCallContext context)
        => await mediator.SendAsync(request, context.CancellationToken).ConfigureAwait(false);

    public override async Task<RoleResponse> UpdateRole(UpdateRoleCommand request, ServerCallContext context)
        => await mediator.SendAsync(request, context.CancellationToken).ConfigureAwait(false);
}