using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Sisa.Constants;

namespace Sisa.Extensions;

public static class ConfigurationPipelineExtensions
{
    public static IApplicationBuilder UseCorsWithoutPolicy(this IApplicationBuilder app)
        => app.UseCors(ApiConstants.CORS_WITHOUT_POLICY);

    public static IApplicationBuilder UseCorsWithPolicy(this IApplicationBuilder app)
        => app.UseCors(ApiConstants.CORS_WITH_POLICY);


    public static IApplicationBuilder UseCustomPathBase(this IApplicationBuilder app, string pathBase)
    {
        if (!string.IsNullOrEmpty(pathBase))
        {
            app.UsePathBase(pathBase);
        }

        return app;
    }

    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()!;

        return app.UseRequestLocalization(localizationOptions.Value);
    }
}
