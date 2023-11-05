using Microsoft.AspNetCore.Identity;

using Sisa.Abstractions;

using Sisa.Constants;
using Sisa.Domain.AggregatesModel.AuditableAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Domain.AggregatesModel.RoleAggregate;

public class Role : IdentityRole<Guid>, IAggregateRoot, IEntity<Guid>, ICreationAuditing, IUpdateAuditing
{
    public string? Description { get; private set; } = string.Empty;
    public bool Predefined { get; private set; }

    public bool Enabled { get; private set; }
    // public string? Remarks { get; private set; }

    public List<string> Permissions { get; } = [];

    private readonly List<UserRole> _userRoles;
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    private readonly List<RoleClaim> _roleClaims;
    public virtual IReadOnlyCollection<RoleClaim> RoleClaims => _roleClaims;

    public Role() : base()
    {
        _userRoles = [];
        _roleClaims = [];

        Enabled = true;
    }

    public Role(string roleName) : base(roleName)
    {
        _userRoles = [];
        _roleClaims = [];
    }

    public Role(string roleName, string description) : this(roleName)
        => Describe(description);

    public Role(string roleName, string description, bool predefined) : this(roleName, description)
        => Predefined = predefined;

    public void MakeAsPredefined() => Predefined = true;

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Describe(string description) => Description = description;

    public void Enable(string remarks)
    {
        Enabled = true;
        // Remarks = remarks;
    }

    public void Disable(string remarks)
    {
        Enabled = false;
        // Remarks = remarks;
    }

    public void UpdatePermissions(IEnumerable<string> permissions)
    {
        IEnumerable<string> permissionsToBeAdded = permissions.Except(Permissions);
        IEnumerable<string> permissionsToBeDeleted = Permissions.Except(permissions);

        _roleClaims.AddRange(permissionsToBeAdded.Select(x => new RoleClaim()
        {
            RoleId = Id,
            ClaimType = SecurityClaimTypes.Permission,
            ClaimValue = x
        }));

        _roleClaims.RemoveAll(x => x.ClaimType == SecurityClaimTypes.Permission && permissionsToBeDeleted.Contains(x.ClaimValue!));

        Permissions.Clear();
        Permissions.AddRange(permissions);
    }

    #region Auditing

    #region Auditing Properties

    public Guid? CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    #endregion

    #region Auditing Methods

    public void AuditCreation(Guid? createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AuditUpdate(Guid? updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    #endregion

    #endregion
}
