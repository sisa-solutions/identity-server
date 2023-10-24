using Sisa.Abstractions;
using Sisa.Domain.AggregatesModel.AuditableAggregate;

namespace Sisa.Domain.AggregatesModel.TenantAggregate;

public class Tenant : FullAuditableAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Tenant(string name, string description) : base()
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
