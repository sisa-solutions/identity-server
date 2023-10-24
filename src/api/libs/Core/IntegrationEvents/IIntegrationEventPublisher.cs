namespace Sisa.Abstractions;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}
