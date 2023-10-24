using OpenIddict.EntityFrameworkCore.Models;

using Sisa.Abstractions;

namespace Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

public class Token : OpenIddictEntityFrameworkCoreToken<Guid, Application, Authorization>, IEntity<Guid>
{
}
