namespace Sisa.Abstractions;

/// <summary>
/// Marker interface for commands
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// Marker interface for commands with a response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse> : ICommand, IRequest<TResponse>
{
}
