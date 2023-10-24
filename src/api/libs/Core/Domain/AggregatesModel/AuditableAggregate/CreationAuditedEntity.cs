using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public class CreationAuditableEntity : Entity, ICreationAuditing
{
    public Guid? CreatedBy { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public void AuditCreation(Guid? createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
