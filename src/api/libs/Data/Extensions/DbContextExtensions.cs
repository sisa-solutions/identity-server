using System.Data;

using Microsoft.EntityFrameworkCore;

using Sisa.Abstractions;

namespace Sisa.Extensions;

public static class DbContextExtensions
{
    // public static async Task DispatchDomainEventsAsync(this IEventPublisher publisher, DbContext dbContext)
    // {
    //     var domainEntities = dbContext.ChangeTracker
    //         .Entries<Entity>()
    //         .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

    //     var domainEvents = domainEntities
    //         .SelectMany(x => x.Entity.DomainEvents)
    //         .ToList();

    //     domainEntities.ToList()
    //         .ForEach(x => x.Entity.ClearDomainEvents());

    //     foreach (var domainEvent in domainEvents)
    //         await publisher.PublishAsync(domainEvent);
    // }

    public static void ValidateInvalidDomainEvents(this IEventPublisher _, DbContext dbContext)
    {
        var invalidDomainEvents = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.All(x => x is not IExecuteBeforeSave || x is not IExecuteAfterSave))
            .SelectMany(x => x.Entity.DomainEvents);

        if (invalidDomainEvents.Any())
            throw new Exception("One or more domain events are invalid.");
    }

    public static async Task DispatchDomainEventsBeforeSaveAsync(this IEventPublisher publisher, DbContext dbContext)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any(x => x is IExecuteBeforeSave));

        var domainEventsMapping = domainEntities
            .Select(x => new
            {
                x.Entity,
                Events = x.Entity.DomainEvents.Where(x => x is IExecuteBeforeSave)
            })
            .ToList();

        foreach (var item in domainEventsMapping)
        {
            foreach (var @event in item.Events)
            {
                await publisher.PublishAsync(@event);

                item.Entity.RemoveDomainEvent(@event);
            }
        }
    }

    public static async Task DispatchDomainEventsAfterSaveAsync(this IEventPublisher publisher, DbContext dbContext)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any(x => x is IExecuteAfterSave));

        var domainEventsMapping = domainEntities
            .Select(x => new
            {
                x.Entity,
                Events = x.Entity.DomainEvents.Where(x => x is IExecuteAfterSave)
            })
            .ToList();

        foreach (var item in domainEventsMapping)
        {
            foreach (var @event in item.Events)
            {
                await publisher.PublishAsync(@event);

                item.Entity.RemoveDomainEvent(@event);
            }
        }
    }
}
