// using Sisa.Abstractions.Mediator;

// namespace Sisa.Abstractions.Services;

// public class NoMediatorService : IMediator
// {
//     public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
//     {
//         return Task.CompletedTask;
//     }

//     public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
//     {
//         return Task.FromResult(default(TResponse)!);
//     }

//     public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
//     {
//         return Task.CompletedTask;
//     }
// }
