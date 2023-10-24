namespace Sisa.Abstractions;

/// <summary>
/// Marker interface for queries.
/// </summary>
public interface IQuery : IRequest
{
}

/// <summary>
/// Marker interface for queries with a response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IQuery, IRequest<TResponse>
{
}
