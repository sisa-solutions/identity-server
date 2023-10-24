namespace Sisa.Abstractions;

public record IntegrationEvent
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public IntegrationEvent(Guid id, DateTimeOffset createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
}
