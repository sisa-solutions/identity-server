namespace Sisa.Abstractions;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    protected AggregateRoot() : base() { }

    protected AggregateRoot(Guid id) : base(id) { }
}
