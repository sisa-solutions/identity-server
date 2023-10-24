using Microsoft.Extensions.DependencyInjection;

using Sisa.Abstractions;

namespace Sisa.Infrastructure.Mediator;

/// <summary>
/// Mediator for queries
/// </summary>
public partial class Mediator(IServiceProvider serviceProvider) : IMediator
{
    /// <summary>
    /// Executes a command
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    {
        var dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();

        await dispatcher.DispatchAsync(command, cancellationToken);
    }

    /// <summary>
    /// Executes a command and returns a response
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<TResponse> SendCommandAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        var dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();

        return await dispatcher.DispatchAsync<TCommand, TResponse>(command, cancellationToken);
    }

    /// <summary>
    /// Executes a query
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<TResponse> SendQueryAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        var dispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();

        return await dispatcher.DispatchAsync<TQuery, TResponse>(query, cancellationToken);
    }

    /// <summary>
    /// Publishes an Event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        var publisher = serviceProvider.GetRequiredService<IEventPublisher>();

        await publisher.PublishAsync(@event, cancellationToken);
    }
}
