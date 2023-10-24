namespace Sisa.Abstractions;

public interface IIntegrationEventHandler
{
}

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event);
}
