using Grpc.AspNetCore.Server;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Sisa.Grpc.Interceptors;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcService(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddGrpc(options => options.Interceptors.Add<ExceptionInterceptor>());

        if (environment.IsDevelopment())
        {
            services.AddGrpcReflection();
        }

        return services;
    }

    public static IServiceCollection AddGrpcService(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        Action<GrpcServiceOptions> configureOptions)
    {
        services.AddGrpc(options =>
        {
            configureOptions(options);

            options.Interceptors.Add<ExceptionInterceptor>();
        });

        if (environment.IsDevelopment())
        {
            services.AddGrpcReflection();
        }

        return services;
    }
}
