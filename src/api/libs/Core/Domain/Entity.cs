namespace Sisa.Abstractions;

public abstract class BaseEntity : IEntity, IDomainEventEntity
{
    private readonly HashSet<IEvent> _domainEvents;
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents;

    protected BaseEntity()
        => _domainEvents ??= [];

    public void AddDomainEvent(IEvent @event)
        => _domainEvents.Add(@event);

    public void RemoveDomainEvent(IEvent @event)
        => _domainEvents.Remove(@event);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}

public abstract class Entity : BaseEntity, IEntity<Guid>
{
    int? _requestedHashCode;

    Guid _id;

    public virtual Guid Id
    {
        get
        {
            return _id;
        }
        protected set
        {
            _id = value;
        }
    }

    protected Entity() : base() { }

    protected Entity(Guid id) : this()
        => Id = id;

    public void SetId(Guid id) => Id = id;

    public bool IsTransient() => Id == Guid.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        Entity item = (Entity)obj;

        if (item.IsTransient() || IsTransient())
            return false;
        else
            return item.Id == Id;
    }

#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
    public override int GetHashCode()
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }
        else
#pragma warning disable S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"
            return base.GetHashCode();
#pragma warning restore S3249 // Classes directly extending "object" should not call "base" in "GetHashCode" or "Equals"

    }

#pragma warning disable S3875 // "operator==" should not be overloaded on reference types
    public static bool operator ==(Entity? left, Entity? right)
#pragma warning restore S3875 // "operator==" should not be overloaded on reference types
    {
        if (Equals(left, null))
            return Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
        => !(left == right);
}
