namespace Sisa.Abstractions;

/// <summary>
/// Marker interface for queries
/// </summary>
public interface IQueryHandler {}

/// <summary>
/// Marker interface for queries with a response
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Handle the query
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
