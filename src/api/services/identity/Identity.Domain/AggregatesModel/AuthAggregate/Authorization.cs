using OpenIddict.EntityFrameworkCore.Models;

using Sisa.Abstractions;

namespace Sisa.Identity.Domain.AggregatesModel.AuthAggregate;

public class Authorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, Application, Token>, IEntity<Guid>
{
}
