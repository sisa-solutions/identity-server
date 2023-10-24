using Sisa.Domain.AggregatesModel.AuditableAggregate;

namespace Sisa.Identity.Domain.PermissionAggregate;

public class PermissionGroup : AuditableEntity
{
    public string Name { get; private set; }

    public string? Description { get; private set; }

    private readonly List<Permission> _permissions = [];

    public virtual IReadOnlyCollection<Permission> Permissions => _permissions;

    public PermissionGroup(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void AddPermission(Permission permission)
    {
        _permissions.Add(permission);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);
    }

    public void ClearPermissions()
    {
        _permissions.Clear();
    }

    public bool HasPermission(string permissionName)
    {
        return _permissions.Any(p => p.Name == permissionName);
    }
}

