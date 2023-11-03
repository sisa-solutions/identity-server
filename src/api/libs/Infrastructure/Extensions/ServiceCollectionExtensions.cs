using System.Globalization;

using FluentEmail.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

using Sisa.Abstractions;
using Sisa.Constants;
using Sisa.Infrastructure.Services;
using Sisa.Settings;

namespace Sisa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorsWithoutPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.CORS_WITHOUT_POLICY, builder =>
            {
                builder.SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddCorsWithPolicy(this IServiceCollection services, params string[] origins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.CORS_WITH_POLICY, builder =>
            {
                builder.WithOrigins(origins)
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddCommonConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions()
            .AddTransient<IIdentityService, IdentityService>();

        services.AddHttpContextAccessor();

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

    public static IServiceCollection AddCustomAntiforgery(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "x-xsrf-token";
            options.FormFieldName = "xsrfToken";
            options.SuppressXFrameOptionsHeader = false;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.HttpOnly = true;
        });

        return services;
    }

    // public static IServiceCollection AddFileStorageService(this IServiceCollection services, AwsSettings settings)
    // {
    //     services.AddSingleton(Options.Create(settings));

    //     services.AddSingleton<IAmazonS3>(_ =>
    //     {
    //         return new AmazonS3Client(settings.AccessKey, settings.SecretKey, new AmazonS3Config()
    //         {
    //             RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region),
    //             ServiceURL = settings.ServiceUrl
    //         });

    //     });

    //     services.AddSingleton<IFileStorageService, FileStorageService>();

    //     return services;
    // }

    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, SmptSettings smptSettings, EmailSender emailSender)
    {
        if (string.IsNullOrEmpty(emailSender.Email))
        {
            throw new ArgumentNullException(nameof(emailSender.Email));
        }

        if (string.IsNullOrEmpty(smptSettings.Host))
        {
            throw new ArgumentNullException(nameof(smptSettings.Host));
        }

        if (string.IsNullOrEmpty(smptSettings.Username))
        {
            throw new ArgumentNullException(nameof(smptSettings.Username));
        }

        if (string.IsNullOrEmpty(smptSettings.Password))
        {
            throw new ArgumentNullException(nameof(smptSettings.Password));
        }

        services
            .AddFluentEmail(emailSender.Email, emailSender.Name)
            .AddRazorRenderer()
            .AddSmtpSender(
                smptSettings.Host,
                smptSettings.Port,
                smptSettings.Username,
                smptSettings.Password);

        return services;
    }
}
