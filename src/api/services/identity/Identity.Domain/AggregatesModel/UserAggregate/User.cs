using Microsoft.AspNetCore.Identity;

using Sisa.Abstractions;
using Sisa.Domain.AggregatesModel.AuditableAggregate;

using Sisa.Enums;

namespace Sisa.Identity.Domain.AggregatesModel.UserAggregate;

public class User : IdentityUser<Guid>, IAggregateRoot, IEntity<Guid>, ICreationAuditing, IUpdateAuditing
{
    [PersonalData]
    public string? FirstName { get; private set; } = string.Empty;

    [PersonalData]
    public string? LastName { get; private set; } = string.Empty;

    [PersonalData]
    public string? FullName { get; private set; } = string.Empty;

    [PersonalData]
    public Gender Gender { get; private set; } = Gender.UNSPECIFIED;

    [PersonalData]
    public DateOnly? BirthDate { get; private set; }

    [PersonalData]
    public string? Picture { get; private set; } = string.Empty;

    public UserStatus Status { get; private set; }

    // public bool IsExternal { get; private set; }

    private readonly List<UserClaim> _userClaims;
    public virtual IReadOnlyCollection<UserClaim> UserClaims => _userClaims;

    private readonly List<UserLogin> _userLogins;
    public virtual IReadOnlyCollection<UserLogin> UserLogins => _userLogins;

    private readonly List<UserToken> _userTokens;
    public virtual IReadOnlyCollection<UserToken> UserTokens => _userTokens;

    private readonly List<UserRole> _userRoles;
    public virtual IReadOnlyCollection<UserRole> UserRoles => _userRoles;

    // private readonly HashSet<SecurityEvent> _securityEvents;
    // public virtual IReadOnlyCollection<SecurityEvent> SecurityEvents => _securityEvents;

    public User(string userName) : base(userName)
    {
        Status = UserStatus.NEW;
        // IsExternal = false;

        _userRoles = new List<UserRole>();

        _userClaims = new List<UserClaim>();
        _userLogins = new List<UserLogin>();
        _userTokens = new List<UserToken>();
        // _securityEvents = new HashSet<SecurityEvent>();
    }

    public User(
        string userName,
        string firstName, string lastName
    ) : this(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{firstName} {lastName}";
    }

    public void UpdatePersonalInfo(
        string? firstName,
        string? lastName,
        string? fullName,
        DateOnly? birthDate,
        Gender gender
    )
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = fullName;

        if (string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            FullName = $"{firstName} {lastName}";

        BirthDate = birthDate;
        Gender = gender;
    }

    public void SetEmail(string email) => Email = email;
    public void ConfirmEmail() => EmailConfirmed = true;
    public void SetPhoneNumber(string phoneNumber) => PhoneNumber = phoneNumber;
    public void ConfirmPhoneNumber() => PhoneNumberConfirmed = true;

    public void UpdateAvatar(string picture)
    {
        Picture = picture;
    }

    // public void MaskAsExternal() => IsExternal = true;

    public IReadOnlyCollection<string> GetRolesName() =>
        _userRoles
            .Where(x => !string.IsNullOrEmpty(x.Role.Name))
            .Select(x => x.Role.Name!).ToList();
    public void AddOrUpdateClaims(IReadOnlyDictionary<string, string> claims)
    {
        IEnumerable<string> requestClaims = claims.Select(x => x.Key);
        IEnumerable<string> existingClaims = _userClaims.Select(x => x.ClaimType!);
        IEnumerable<string> claimsToBeAdded = requestClaims.Except(existingClaims);
        IEnumerable<string> claimsToBeDeleted = existingClaims.Except(requestClaims);
        IEnumerable<string> claimsToBeUpdated = existingClaims.Intersect(requestClaims);

        _userClaims.AddRange(claimsToBeAdded.Select(claimType => new UserClaim()
        {
            ClaimType = claimType,
            ClaimValue = claims[claimType]
        }));

        _userClaims.RemoveAll(x => claimsToBeDeleted.Any(c => c == x.ClaimType));

        foreach (var claimType in claimsToBeUpdated)
        {
            foreach (var claim in _userClaims)
            {
                if (claim.ClaimType == claimType)
                    claim.ClaimValue = claims[claimType];
            }
        }
    }

    public IReadOnlyDictionary<string, string> GetExtraInfo() =>
        _userClaims.GroupBy(g => g.ClaimType).Select(s => s.First())
                   .ToDictionary(x => x.ClaimType!, x => x.ClaimValue!);

    #region Status

    public UserStatus[] AllowedNextStatues => Status switch
    {
        UserStatus.NEW => [UserStatus.REGISTERED],
        UserStatus.REGISTERED => [UserStatus.ACTIVE, UserStatus.INACTIVE],
        UserStatus.ACTIVE => [UserStatus.PASSWORD_EXPIRED, UserStatus.LOCKED_OUT, UserStatus.INACTIVE, UserStatus.SUSPENDED],
        UserStatus.LOCKED_OUT => [UserStatus.RECOVERY],
        UserStatus.INACTIVE => [UserStatus.RECOVERY],
        UserStatus.SUSPENDED => [UserStatus.RECOVERY],
        UserStatus.PASSWORD_EXPIRED => [UserStatus.RECOVERY],
        UserStatus.RECOVERY => [UserStatus.ACTIVE, UserStatus.INACTIVE,],
        _ => [],
    };

    public bool IsAllowedToChangeStatus(UserStatus nextStatus)
        => AllowedNextStatues.Contains(nextStatus);

    public bool TryToChangeStatus(UserStatus status, string? remarks)
    {
        if (!IsAllowedToChangeStatus(status))
            return false;

        Status = status;

        return true;
    }

    public bool UnblockAccount(string? remarks)
    {
        LockoutEnd = null;
        return TryToChangeStatus(UserStatus.ACTIVE, remarks);
    }

    public bool BlockAccount(int lockoutDuration, string? remarks)
    {
        LockoutEnd = DateTimeOffset.UtcNow.AddHours(lockoutDuration);
        return TryToChangeStatus(UserStatus.LOCKED_OUT, remarks);
    }

    #endregion

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
