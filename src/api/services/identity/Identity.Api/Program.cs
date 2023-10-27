using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

using Sisa.Extensions;

using Sisa.Identity.Api.V1.Account;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonConfiguration();

builder.Services.AddMediator();
builder.Services.AddMediatorDependencies();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "x-xsrf-token";
    options.SuppressXFrameOptionsHeader = false;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

app.UseAntiforgery();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value ?? string.Empty;
    string[] fontendPaths = ["/login"];

    if (fontendPaths.Any(x => requestPath.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append("x-xsrf-token", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.Strict });
    }

    return next(context);
});

app.MapAccountEndpoint();

if (!app.Environment.IsDevelopment())
{
    app.MapFallbackToFile("index.html");
}
else
{
    app.MapWhen(context =>
        context.Request.Path.Value?.StartsWith("/api") == false &&
        context.Request.Path.Value?.StartsWith("/connect") == false &&
        context.Request.Path.Value?.StartsWith("/account") == false,

        builder => builder.UseSpa(configuration =>
            {
                configuration.Options.DevServerPort = 10002;
                configuration.Options.PackageManagerCommand = "yarn";
                configuration.Options.SourcePath = "../../../../web";
                configuration.Options.StartupTimeout = TimeSpan.FromSeconds(10);
                configuration.UseReactDevelopmentServer("dev:identity");
            }));
}

await app.RunAsync();
