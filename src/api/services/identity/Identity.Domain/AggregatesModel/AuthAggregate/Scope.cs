using OpenIddict.EntityFrameworkCore.Models;

using Sisa.Abstractions;

namespace Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

public class Scope : OpenIddictEntityFrameworkCoreScope<Guid>, IEntity<Guid>
{
}
