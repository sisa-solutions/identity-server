using Microsoft.Extensions.DependencyInjection;

using Sisa.Abstractions;

namespace Sisa.Infrastructure.Mediator;

/// <summary>
/// Dispatches a query to the appropriate handler.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="QueryDispatcher"/> class.
/// </remarks>
/// <param name="serviceProvider"></param>
public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    /// <summary>
    /// Dispatches a query to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<TResponse> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        IEnumerable<IRequestPipeline<TQuery, TResponse>> pipelines = serviceProvider.GetServices<IRequestPipeline<TQuery, TResponse>>();
        IQueryHandler<TQuery, TResponse> handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        if (!pipelines.Any())
        {
            return await handler.HandleAsync(query, cancellationToken);
        }
        else
        {
            async ValueTask<TResponse> HandlerDelegate() => await handler.HandleAsync(query, cancellationToken);

            return await pipelines.OrderByDescending(p => p.Priority)
                .Aggregate(
                    (RequestHandlerDelegate<TResponse>)HandlerDelegate,
                    (next, pipeline) => async () => await pipeline.HandleAsync(query, next, cancellationToken)
                )();
        }
    }
}
