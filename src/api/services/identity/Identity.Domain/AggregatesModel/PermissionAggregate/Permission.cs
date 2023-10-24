using Sisa.Domain.AggregatesModel.AuditableAggregate;

namespace Sisa.Identity.Domain.PermissionAggregate;

public class Permission : CreationAuditableAggregateRoot
{
    public Guid GroupId { get; init; }

    public string Value { get; init; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public virtual PermissionGroup Group { get; private set; } = null!;

    public Permission(string value, string name, string description)
    {
        Value = value;
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

