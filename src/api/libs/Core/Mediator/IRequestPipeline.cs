namespace Sisa.Abstractions;

/// <summary>
/// Handles a Request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <param name="request"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public delegate Task RequestHandlerDelegate();

/// <summary>
/// Handles a Request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="request"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
public delegate ValueTask<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Handles a Request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public interface IRequestPipeline<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// The priority of the pipeline.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles a Request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default
    );
}

/// <summary>
/// Handles a Request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestPipeline<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// The priority of the pipeline.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Handles a Request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="handler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default
    );
}
