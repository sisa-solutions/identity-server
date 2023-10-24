using Sisa.Abstractions;

namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public class FullAuditableEntity : AuditableEntity, IDeletionAuditing
{
    public Guid? DeletedBy { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public void AuditDeletion(Guid? deletedBy)
    {
        DeletedBy = deletedBy;
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
