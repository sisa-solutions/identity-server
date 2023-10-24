using Sisa.Domain.AggregatesModel.AuditableAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

namespace Sisa.Identity.Domain.AggregatesModel.SecurityEventAggregate;

public class SecurityEvent : CreationAuditableAggregateRoot
{
    #region Context

    public string? Ip { get; private set; }
    public string? Agent { get; private set; }
    public string? Culture { get; private set; }
    public string? Protocol { get; private set; }
    public string? Schema { get; private set; }
    public string? Origin { get; private set; }
    public string? Uri { get; private set; }
    public string? Method { get; private set; }
    public string? CorrelationId { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public string? ApplicationName { get; private set; }

    #endregion

    #region User Information

    public Guid? TenantId { get; private set; }
    public string? TenantName { get; private set; }

    public Guid UserId { get; private set; }
    public string? UserName { get; private set; }

    #endregion

    #region Details

    public DateTimeOffset StartTime { get; private set; }
    public int Duration { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public int? StatusCode { get; private set; }
    public string? Exception { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    #endregion

    public string? Remarks { get; private set; }

    public SecurityAction Action { get; private set; }

    public virtual User User { get; private set; } = null!;

    public void SetRemarks(string? remarks)
        => Remarks = remarks;
}
