using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

using Npgsql;

using Sisa.Constants;
using Sisa.Extensions;
using Sisa.Helpers;
using Sisa.Identity.Api.V1.Account;
using Sisa.Identity.Data;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Settings;

using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

var connectionString = builder.Configuration.GetConnectionString(SchemaConstants.DEFAULT_CONNECTION)!;
var distributedCachingConnectionString = builder.Configuration.GetConnectionString(SchemaConstants.DISTRIBUTED_CACHING_CONNECTION)!;

builder.Services.AddCommonConfiguration()
    .AddCorsWithPolicy(appSettings.CorsPolicy.GetOrigins());

builder.Services.AddMediator();
builder.Services.AddMediatorDependencies();
builder.Services.AddDataDependencies();

if (!builder.Environment.IsProduction())
{
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    // builder.Services.AddDistributedRedisCache(distributedCachingConnectionString, appSettings.DistributedCacheInstance);
}

#region DbContext

var certs = await CertificateHelper.GetCertificatesFromPathsAsync(appSettings.Certs.DataProtection, appSettings.Certs.EncryptingSigning);
var dpCerts = certs[0];
var identityCerts = certs[1];

var dataSourceBuilder = new NpgsqlSlimDataSourceBuilder(connectionString);

dataSourceBuilder.EnableDynamicJsonMappings(jsonbClrTypes: [typeof(IReadOnlyCollection<string>)]);
dataSourceBuilder.EnableArrays();

var dataSource = dataSourceBuilder.Build();

builder.Services
    .AddDbContextFactory<IdentityDbContext>((serviceProvider, options)
        => options.UseDatabase(serviceProvider, dataSource));

#endregion


#region Identity

builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 3;

        // Note: to require account confirmation before login,
        // register an email sender service (IEmailSender) and
        // options.SignIn.RequireConfirmedAccount = true;
        // options.SignIn.RequireConfirmedEmail = true;

        options.ClaimsIdentity.UserIdClaimType = SecurityClaimTypes.Subject;
        options.ClaimsIdentity.UserNameClaimType = SecurityClaimTypes.Name;
        options.ClaimsIdentity.RoleClaimType = SecurityClaimTypes.Role;
        options.ClaimsIdentity.EmailClaimType = SecurityClaimTypes.Email;

        // For more information, visit https://aka.ms/aspaccountconf.
        options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
        options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    })
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<CookieAuthenticationOptions>(options =>
{
    options.ReturnUrlParameter = "return_url";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
});

builder.Services
    .AddAuthorization()
    .ConfigureApplicationCookie(options =>
    {
        options.ReturnUrlParameter = "return_url";
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    })
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(2);
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });


#endregion

#region OpenIddict

builder.Services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<IdentityDbContext>()
            .ReplaceDefaultEntities<Application, Authorization, Scope, Token, Guid>();
    })
    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("connect/authorize", "connect/context", "connect/consent")
            .SetTokenEndpointUris("connect/token")
            .SetIntrospectionEndpointUris("connect/introspect")
            .SetDeviceEndpointUris("connect/device")
            .SetVerificationEndpointUris("connect/verify")
            .SetLogoutEndpointUris("connect/endsession")
            .SetRevocationEndpointUris("connect/revocation")
            .SetUserinfoEndpointUris("connect/userinfo");

        options.SetIssuer(appSettings.Identity.Authority);

        // Enable the authorization code, implicit and the refresh token flows.
        options.AllowAuthorizationCodeFlow()
            .AllowPasswordFlow()
            .AllowDeviceCodeFlow()
            .AllowClientCredentialsFlow()
            .AllowRefreshTokenFlow();

        options.UseReferenceAccessTokens()
            .UseReferenceRefreshTokens();

        // Expose all the supported scopes in the discovery document.
        options.RegisterScopes(
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Phone,
            Scopes.Profile,
            Scopes.OfflineAccess
        );

        // Register the signing and encryption credentials used to protect
        // sensitive data like the state tokens produced by OpenIddict.
        options.AddEncryptionCertificate(identityCerts[0])
            .AddSigningCertificate(identityCerts[1]);

        if (builder.Environment.IsDevelopment())
            options.DisableAccessTokenEncryption();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        //
        // Note: the pass-through mode is not enabled for the token endpoint
        // so that token requests are automatically handled by OpenIddict.

        var openIddictServerBuilder = options
            .UseAspNetCore()
            .EnableStatusCodePagesIntegration()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableAuthorizationRequestCaching()
            .EnableVerificationEndpointPassthrough();

        if (builder.Environment.IsDevelopment())
            openIddictServerBuilder.DisableTransportSecurityRequirement();

        // Register the event handler responsible for populating userinfo responses.
        // options.AddEventHandler<HandleUserinfoRequestContext>(configuration =>
        //     configuration.UseSingletonHandler<PopulateUserInfoHandler>());
    })
    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        options.SetIssuer(appSettings.Identity.Authority);

        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Instead of validating the token locally by reading it directly,
        // introspection can be used to ask a remote authorization server
        // to validate the token (and its attached database entry).
        //
        // options.UseIntrospection()
        //        .SetIssuer("https://localhost:44395/")
        //        .SetClientId("resource_server")
        //        .SetClientSecret("80B552BB-4CD8-48DA-946E-0815E0147DD2");
        //
        // When introspection is used, System.Net.Http integration must be enabled.
        //
        // options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();

        // For applications that need immediate access token or authorization
        // revocation, the database entry of the received tokens and their
        // associated authorizations can be validated for each API call.
        // Enabling these options may have a negative impact on performance.
        //
        options.EnableAuthorizationEntryValidation();
        // options.EnableTokenEntryValidation();
    });

#endregion

builder.Services.AddCustomAntiforgery();

var app = builder.Build();

app.UseCustomPathBase(appSettings.PathBase);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    var issuer = new Uri(appSettings.Identity.Authority);

    app.Use((context, next) =>
    {
        context.Request.Scheme = "https";
        context.Request.Host = new HostString(issuer.Host);

        return next(context);
    });

    app.UseForwardedHeaders();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStatusCodePages();

app.UseAntiforgery();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value ?? string.Empty;
    string[] frontendPaths = ["/login"];

    if (frontendPaths.Any(x => requestPath.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append("x-xsrf-token", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.Strict });
    }

    return next(context);
});

app.UseRouting();
app.UseCorsWithPolicy();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseResponseCaching();
app.UseResponseCompression();

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
