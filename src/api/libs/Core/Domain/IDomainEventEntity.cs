namespace Sisa.Abstractions;

public interface IDomainEventEntity
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }

    void AddDomainEvent(IEvent @event);

    void RemoveDomainEvent(IEvent @event);

    void ClearDomainEvents();
}

public interface IDomainEvent : IEvent
{
}

public interface IExecuteBeforeSave
{
}

public interface IExecuteAfterSave
{
}