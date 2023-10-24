using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public class AuditableEntity : Entity, ICreationAuditing, IUpdateAuditing
{
    public Guid? CreatedBy { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public Guid? UpdatedBy { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

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
}
