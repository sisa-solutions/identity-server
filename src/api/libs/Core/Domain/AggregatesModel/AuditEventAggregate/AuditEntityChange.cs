using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditEventAggregate;

public class AuditEntityChange : Entity
{
    public Guid AuditEventId { get; private set; }
    public string EntityId { get; private set; }
    public string EntityName { get; private set; }
    public string EntityFullName { get; private set; }
    public EntityChangeType ChangeType { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    public string? Remarks { get; private set; } = string.Empty;

    public virtual AuditEvent AuditEvent { get; private set; } = null!;

    public AuditEntityChange(string entityId, string entityName, string entityFullName, EntityChangeType changeType)
    {
        EntityId = entityId;
        EntityName = entityName;
        EntityFullName = entityFullName;
        ChangeType = changeType;

        ChangedAt = DateTimeOffset.UtcNow;
    }

    public void TrackChange(string? oldValues, string? newValues, string? remarks)
    {
        OldValues = oldValues;
        NewValues = newValues;
        Remarks = remarks;
    }
}
