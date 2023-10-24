namespace Sisa.Abstractions;

/// <summary>
/// Marker interface for requests
/// </summary>
public interface IRequest
{
}

/// <summary>
/// Marker interface for requests with a response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<TResponse> : IRequest
{
}
