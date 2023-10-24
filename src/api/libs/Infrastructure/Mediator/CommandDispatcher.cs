using Microsoft.Extensions.DependencyInjection;

using Sisa.Abstractions;

namespace Sisa.Infrastructure.Mediator;

/// <summary>
/// Dispatches a Command to the appropriate handler.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
/// </remarks>
/// <param name="serviceProvider"></param>
public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    /// <summary>
    /// Dispatches a Command to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        IEnumerable<IRequestPipeline<TCommand>> pipelines = serviceProvider.GetServices<IRequestPipeline<TCommand>>();
        ICommandHandler<TCommand> handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        if (!pipelines.Any())
        {
            await handler.HandleAsync(command, cancellationToken);
        }
        else
        {
            async Task HandlerDelegate() => await handler.HandleAsync(command, cancellationToken);

            await pipelines.OrderByDescending(p => p.Priority)
                .Aggregate(
                    (RequestHandlerDelegate)HandlerDelegate,
                    (next, pipeline) => async () => await pipeline.HandleAsync(command, next, cancellationToken)
                )();
        }
    }

    /// <summary>
    /// Dispatches a Command to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        IEnumerable<IRequestPipeline<TCommand, TResponse>> pipelines = serviceProvider.GetServices<IRequestPipeline<TCommand, TResponse>>();
        ICommandHandler<TCommand, TResponse> handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

        if (!pipelines.Any())
        {
            return await handler.HandleAsync(command, cancellationToken);
        }
        else
        {
            async ValueTask<TResponse> HandlerDelegate() => await handler.HandleAsync(command, cancellationToken);

            return await pipelines.OrderByDescending(p => p.Priority)
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)HandlerDelegate,
                    (next, pipeline) => async () => await pipeline.HandleAsync(command, next, cancellationToken)
                )();
        }
    }
}
