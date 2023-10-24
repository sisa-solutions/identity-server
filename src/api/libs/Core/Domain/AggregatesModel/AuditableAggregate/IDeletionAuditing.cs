namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public interface IDeletionAuditing
{
    Guid? DeletedBy { get; }
    DateTimeOffset? DeletedAt { get; }

    void AuditDeletion(Guid? deletedBy);
}
