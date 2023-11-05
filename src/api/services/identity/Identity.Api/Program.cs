using Sisa.Identity.Data;
using Sisa.Extensions;
using Sisa.Identity.Settings;
using Sisa.Constants;
using Sisa.Identity.Domain.AggregatesModel.AuthAggregate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Npgsql;
using Sisa.Helpers;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

var connectionString = builder.Configuration.GetConnectionString(SchemaConstants.DEFAULT_CONNECTION)!;
var distributedCachingConnectionString = builder.Configuration.GetConnectionString(SchemaConstants.DISTRIBUTED_CACHING_CONNECTION)!;

// Add services to the container.
builder.Services.AddCommonConfiguration()
    .AddCorsWithPolicy(appSettings.CorsPolicy.GetOrigins());

builder.Services.AddGrpcService(builder.Environment);

builder.Services.AddFluentValidation();
builder.Services.AddMediator();
builder.Services.AddMediatorDependencies();
builder.Services.AddDataDependencies();
builder.Services.AddInfrastructureDependencies();
// builder.Services.AddFileStorageService(
//     new Sisa.Infrastructure.Settings.AwsSettings(
//         serviceUrl: "https://48cc388ce8732400b061994f2395fb01.r2.cloudflarestorage.com",
//         region: "us-east-1",
//         accessKey: "96d5ba19024eb350ee856dfd031d26b3",
//         secretKey: "96ef6d4686f29ef1cfa1e209bee29dcb3a2327660f88a7404a77bae03282c915",
//         chunkSize: 5_242_880L,
//         disablePayloadSigning: true,
//         defaultBucket: "sisa-cafe-local"
//     )
// );

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
    .AddPooledDbContext<IdentityDbContext, IdentityDbContextFactory>((serviceProvider, options)
        => options.UseDatabase(serviceProvider, dataSource)
    ).AddDataProtectionContext<IdentityDbContext>(appSettings.AppInstance, 90, dpCerts);

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

builder.Services
    .AddAuthorization()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

#endregion

#region OpenIddict

builder.Services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<IdentityDbContext>()
            .ReplaceDefaultEntities<Application, Authorization, Scope, Token, Guid>();
    });

#endregion

builder.Services.AddSmtpEmailSender(appSettings.Email.Smtp, appSettings.Email.Sender);

var app = builder.Build();

app.UseCustomPathBase(appSettings.PathBase);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStatusCodePages();
app.UseCorsWithPolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCaching();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcServices();

// if (app.Environment.IsDevelopment())
// {
app.MapGrpcReflectionService();
// }

await app.RunAsync();
