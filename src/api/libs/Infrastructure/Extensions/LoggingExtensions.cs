// using Microsoft.Extensions.Hosting;

// using Serilog;
// using Serilog.Events;

// namespace Microsoft.Extensions.Logging;

// public static class LoggingExtensions
// {
//     public static IHostBuilder UseLogger(this IHostBuilder host) =>
//         host.UseSerilog((context, services, configuration) =>
//         {
//             var loggerConfiguration = configuration
//                 .ReadFrom.Configuration(context.Configuration)
//                 .Enrich.FromLogContext()
//                 .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
//                 .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
//                 .MinimumLevel.Information();

//             if (!context.HostingEnvironment.IsDevelopment())
//             {
//                 loggerConfiguration
//                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//                     .MinimumLevel.Override("System", LogEventLevel.Warning)
//                     .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
//                     .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Information)
//                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning);
//             }

//             loggerConfiguration.Destructure.With(new DefaultSensitiveDataDestructuringPolicy(context.Configuration));
//         });
// }
