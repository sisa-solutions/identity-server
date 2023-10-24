namespace Sisa.Identity.Domain.AggregatesModel.UserAggregate;

public enum UserStatus : byte
{
    NEW,

    /// <summary>
    /// Accounts have a registered status when the user has been created but has not yet been verified.
    /// The user cannot access applications.
    /// </summary>
    REGISTERED,

    /// <summary>
    /// Accounts have an active status when the user has been created and is able to access applications.
    /// </summary>
    ACTIVE,

    /// <summary>
    /// Accounts have a locked out status when the user has exceeded the maximum number of failed login attempts.
    /// The user cannot access applications.
    /// </summary>
    LOCKED_OUT,

    /// <summary>
    /// Accounts have a password expired status when the user has not changed their password within the specified time period.
    /// The user cannot access applications.
    /// </summary>
    PASSWORD_EXPIRED,

    /// <summary>
    /// Accounts have an inactive status when the user has been deactivated by an administrator.
    /// The user cannot access applications.
    /// </summary>
    INACTIVE,

    /// <summary>
    /// Accounts have a recovery status when the user has requested a password reset.
    /// The user cannot access applications.
    /// </summary>
    RECOVERY,

    /// <summary>
    /// Accounts have a suspended status when the user has been suspended by an administrator.
    /// The user cannot access applications.
    /// </summary>
    SUSPENDED
}
