namespace Sisa.Domain.AggregatesModel.AuditableAggregate;

public interface ICreationAuditing
{
    Guid? CreatedBy { get; }
    DateTimeOffset CreatedAt { get; }

    void AuditCreation(Guid? createdBy);
}
