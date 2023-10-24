using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditEventAggregate;

public class AuditEvent : AggregateRoot
{
    #region Context

    public string? Ip { get; private set; }
    public string? Agent { get; private set; }
    public string? Culture { get; private set; }
    public string? Protocol { get; private set; }
    public string? Scheme { get; private set; }
    public string? Origin { get; private set; }
    public string? Uri { get; private set; }
    public string? Method { get; private set; }
    public string? CorrelationId { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public string? ApplicationName { get; private set; }
    public string? ModuleName { get; private set; }

    #endregion

    #region User Information

    public Guid? TenantId { get; private set; }
    public string? TenantName { get; private set; }

    public Guid? UserId { get; private set; }
    public string? UserName { get; private set; }

    public Guid? ImpersonalTenantId { get; private set; }
    public string? ImpersonalTenantName { get; private set; }

    public Guid? ImpersonalUserId { get; private set; }
    public string? ImpersonalUserName { get; private set; }

    #endregion

    #region Details

    public DateTimeOffset StartTime { get; private set; }
    public int Duration { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public string? StatusCode { get; private set; }
    public string? Exception { get; private set; }

    #endregion

    public string? Remarks { get; private set; }

    private readonly HashSet<AuditAction> _auditActions;
    public IReadOnlyCollection<AuditAction> AuditActions => _auditActions;

    private readonly HashSet<AuditEntityChange> _auditEntityChanges;
    public IReadOnlyCollection<AuditEntityChange> AuditEntityChanges => _auditEntityChanges;

    public AuditEvent()
    {
        _auditActions = new HashSet<AuditAction>();
        _auditEntityChanges = new HashSet<AuditEntityChange>();

        StartTime = DateTimeOffset.UtcNow;
    }

    public void SetRemarks(string? remarks)
        => Remarks = remarks;
}
