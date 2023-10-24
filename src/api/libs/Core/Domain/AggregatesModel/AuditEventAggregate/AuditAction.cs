using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditEventAggregate;

public class AuditAction : Entity
{
    public Guid AuditEventId { get; private set; }
    public string ServiceName { get; private set; }
    public string MethodName { get; private set; }
    public string? Parameters { get; private set; }
    public string? ReturnData { get; private set; }

    public DateTimeOffset StartTime { get; private set; }
    public int Duration { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public string? Exception { get; private set; } = string.Empty;

    public bool IsManual { get; private set; }
    public string? Remarks { get; private set; } = string.Empty;

    public virtual AuditEvent AuditEvent { get; private set; } = null!;

    public AuditAction(string serviceName, string methodName)
    {
        ServiceName = serviceName;
        MethodName = methodName;

        StartTime = DateTimeOffset.UtcNow;
        IsManual = false;
    }

    public void MarkAsManual(string? remarks)
    {
        IsManual = true;
        Remarks = remarks;
    }
}
