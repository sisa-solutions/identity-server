// using Microsoft.Extensions.Logging;

// using Sisa.Abstractions.Mediator;

// namespace Sisa.Abstractions.Services;

// public sealed class DefaultServices
// {
//     private DefaultServices() { }

//     public static IIdentityService IdentityService
//         => new NoIdentityService();

//     public static IMediator MediatorService
//         => new NoMediatorService();

//     public static ILogger<TCategory> Logger<TCategory>()
//     {
//         var loggerFactory = LoggerFactory.Create(_ => { });
//         var logger = loggerFactory.CreateLogger<TCategory>();

//         return logger;
//     }
// }
