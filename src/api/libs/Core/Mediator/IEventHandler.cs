namespace Sisa.Abstractions;

public interface IEventHandler {}

/// <summary>
/// Handles events from the event bus.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Handles an event from the event bus.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
