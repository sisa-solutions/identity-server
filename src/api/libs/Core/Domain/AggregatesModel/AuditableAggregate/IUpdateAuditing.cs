namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public interface IUpdateAuditing
{
    Guid? UpdatedBy { get; }
    DateTimeOffset? UpdatedAt { get; }

    void AuditUpdate(Guid? updatedBy);
}
