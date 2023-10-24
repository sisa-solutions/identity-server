using System.Globalization;

using Amazon;
using Amazon.S3;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Sisa.Abstractions;
using Sisa.Infrastructure.Services;
using Sisa.Infrastructure.Settings;

// using Sisa.Abstractions.DependencyInjection;
// using Sisa.Abstractions.Services;
// using Sisa.Services;

namespace Sisa.Extensions;

public static class ServiceCollectionExtensions
{
    // public static IServiceCollection AddCorsWithoutPolicy(this IServiceCollection services)
    // {
    //     services.AddCors(options =>
    //     {
    //         options.AddPolicy(ApiConstants.CORS_WITHOUT_POLICY, builder =>
    //         {
    //             builder.SetIsOriginAllowed((host) => true)
    //                 .AllowAnyMethod()
    //                 .AllowAnyHeader()
    //                 .AllowCredentials();
    //         });
    //     });

    //     return services;
    // }

    // public static IServiceCollection AddCorsWithPolicy(this IServiceCollection services, params string[] origins)
    // {
    //     services.AddCors(options =>
    //     {
    //         options.AddPolicy(ApiConstants.CORS_WITH_POLICY, builder =>
    //         {
    //             builder.WithOrigins(origins)
    //                 .SetIsOriginAllowedToAllowWildcardSubdomains()
    //                 .AllowAnyMethod()
    //                 .AllowAnyHeader()
    //                 .AllowCredentials();
    //         });
    //     });

    //     return services;
    // }

    public static IServiceCollection AddCommonConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddTransient<IIdentityService, IdentityService>();

        services.AddResponseCaching();
        services.AddResponseCompression();

        return services;
    }

    public static IServiceCollection ConfigureLocalization(this IServiceCollection services, string defaultCulture, string[] supportedCultures)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var cultures = new List<CultureInfo>();

            foreach (var culture in supportedCultures)
            {
                cultures.Add(new CultureInfo(culture));
            }

            options.DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture);
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
            options.FallBackToParentUICultures = true;
            options.ApplyCurrentCultureToResponseHeaders = true;

            options.RequestCultureProviders = new List<IRequestCultureProvider>()
            {
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        return services;
    }

    public static IServiceCollection AddFileStorageService(this IServiceCollection services, AwsSettings settings)
    {
        services.AddSingleton(Options.Create(settings));

        services.AddSingleton<IAmazonS3>(_ =>
        {
            return new AmazonS3Client(settings.AccessKey, settings.SecretKey, new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region),
                ServiceURL = settings.ServiceUrl
            });

        });

        services.AddSingleton<IFileStorageService, FileStorageService>();

        return services;
    }
}
