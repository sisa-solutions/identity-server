namespace Sisa.Abstractions;

/// <summary>
/// Dispatches a Command to the appropriate handler.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Dispatches a Command to the appropriate handler without returning a result.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    /// <summary>
    /// Dispatches a Command to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TResponse> DispatchAsync<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>;
}
