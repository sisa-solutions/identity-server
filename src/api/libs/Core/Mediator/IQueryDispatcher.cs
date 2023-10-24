namespace Sisa.Abstractions;

/// <summary>
/// Dispatches a query to the appropriate handler.
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    /// Dispatches a query to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TResponse> DispatchAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>;
}
