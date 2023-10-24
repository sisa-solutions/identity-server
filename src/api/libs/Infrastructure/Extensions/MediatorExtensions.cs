using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Sisa.Abstractions;
using Sisa.Infrastructure.Mediator;

namespace Sisa.Extensions;

public static partial class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.TryAddTransient<ICommandDispatcher, CommandDispatcher>();
        services.TryAddTransient<IQueryDispatcher, QueryDispatcher>();
        services.TryAddTransient<IEventPublisher, EventPublisher>();

        services.TryAddTransient<IMediator, Mediator>();

        return services;
    }
}
