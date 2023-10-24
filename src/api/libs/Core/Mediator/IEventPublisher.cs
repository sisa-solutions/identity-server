namespace Sisa.Abstractions;

/// <summary>
/// Publishes an Event to all registered handlers.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an Event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
