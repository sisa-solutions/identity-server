using System.Globalization;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using OpenIddict.Abstractions;

using Sisa.Identity.Data;
using Sisa.Identity.Domain.AggregatesModel.RoleAggregate;
using Sisa.Identity.Domain.AggregatesModel.UserAggregate;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Sisa.Identity.DbMigrator.Seeds
{
    public sealed class Seed_Release_001
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IdentityDbContext dbContext,
            ILogger<IdentityDbContext> logger)
        {
            await InitRoles(services);
            await InitUsers(services);
            await InitApplications(services);
            await InitResources(services);
            await InitScopes(services);
        }

        private static async Task InitRoles(IServiceProvider serviceProvider)
        {
            var roleInfos = new RoleInfo[] {
                new("Administrator", "Administrator", true),
            };

            await InitRoles(serviceProvider, roleInfos);
        }

        private static async Task InitRoles(IServiceProvider serviceProvider, params RoleInfo[] roleInfos)
        {
            var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var roleInfo in roleInfos)
            {
                if (await roleManager.Roles.AnyAsync(t => t.NormalizedName == roleInfo.Name.ToUpper()))
                    continue;

                var role = new Role(roleInfo.Name, roleInfo.Description, roleInfo.Predefined);

                role.AuditCreation(null);

                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    continue;
            }
        }

        private static async Task InitUsers(IServiceProvider serviceProvider)
        {
            var usersInfo = new UserInfo[] {
                new("administrator", "P#ssw0rd", "Administrator", "System", "Admin", "administrator@sisa.io", "0123456789"),
            };

            await InitUsers(serviceProvider, usersInfo);
        }

        private static async Task InitUsers(IServiceProvider serviceProvider, params UserInfo[] usersInfo)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            foreach (var userInfo in usersInfo)
            {
                var user = await userManager.FindByNameAsync(userInfo.UserName);

                if (user != null) return;

                user = new User(
                    userInfo.UserName,
                    userInfo.FirstName,
                    userInfo.LastName
                );

                user.SetEmail(userInfo.Email);
                user.SetPhoneNumber(userInfo.PhoneNumber);

                user.ConfirmEmail();
                user.ConfirmPhoneNumber();

                user.AuditCreation(null);

                user.TryToChangeStatus(UserStatus.ACTIVE, string.Empty);

                var result = await userManager.CreateAsync(user, userInfo.Password);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = await userManager.AddToRoleAsync(user, userInfo.RoleName);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static async Task InitApplications(IServiceProvider serviceProvider)
        {
            var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("api-docs") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ApplicationType = ApplicationTypes.Web,
                    ClientType = ClientTypes.Public,
                    ClientId = "api-docs",
                    DisplayName = "Api Docs",
                    ConsentType = ConsentTypes.Implicit,
                    RedirectUris = {
                        new Uri("http://localhost:12000/api-docs/oauth2-redirect.html"),
                    },
                    PostLogoutRedirectUris = {
                        new Uri("http://localhost:12000/api-docs"),
                    },
                    Permissions = {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Revocation,
                        Permissions.Endpoints.Logout,

                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,

                        Permissions.ResponseTypes.Code,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Phone,
                        Permissions.Scopes.Roles,
                        Permissions.Scopes.Profile,

                        Permissions.Prefixes.Scope + "api-gateway.all",

                        Permissions.Prefixes.Scope + "identity.all",
                        Permissions.Prefixes.Scope + "notification.all",
                        Permissions.Prefixes.Scope + "file-storage.all",
                    },

                    Settings =
                    {
                        // Use a shorter access token lifetime for tokens issued to the Api Docs application.
                        [OpenIddictConstants.Settings.TokenLifetimes.AccessToken] = TimeSpan.FromMinutes(20).ToString("c", CultureInfo.InvariantCulture)
                    },

                    Requirements = {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }

            if (await manager.FindByClientIdAsync("admin-portal") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ApplicationType = ApplicationTypes.Web,
                    ClientType = ClientTypes.Public,
                    ClientId = "admin-portal",
                    DisplayName = "Admin Portal",
                    ConsentType = ConsentTypes.Implicit,
                    RedirectUris = {
                        new Uri("http://localhost:11000/signin-callback"),
                    },
                    PostLogoutRedirectUris = {
                        new Uri("http://localhost:11000"),
                    },
                    Permissions = {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Revocation,
                        Permissions.Endpoints.Logout,

                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,

                        Permissions.ResponseTypes.Code,

                        Permissions.Prefixes.Scope + Scopes.OpenId,
                        Permissions.Prefixes.Scope + Scopes.OfflineAccess,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Phone,
                        Permissions.Scopes.Roles,
                        Permissions.Scopes.Profile,

                        Permissions.Prefixes.Scope + "api-gateway.all",

                        Permissions.Prefixes.Scope + "identity.all",
                        Permissions.Prefixes.Scope + "notification.all",
                        Permissions.Prefixes.Scope + "file-storage.all",
                    },
                    Requirements = {
                        Requirements.Features.ProofKeyForCodeExchange
                    },
                });
            }

            if (await manager.FindByClientIdAsync("web") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ApplicationType = ApplicationTypes.Web,
                    ClientType = ClientTypes.Public,
                    ClientId = "Web",
                    DisplayName = "Web",
                    ConsentType = ConsentTypes.Explicit,
                    RedirectUris = {
                        new Uri("http://localhost:11010/signin-callback"),
                    },
                    PostLogoutRedirectUris = {
                        new Uri("http://localhost:11010"),
                    },
                    Permissions = {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Revocation,
                        Permissions.Endpoints.Logout,

                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,

                        Permissions.ResponseTypes.Code,

                        Permissions.Prefixes.Scope + Scopes.OpenId,
                        Permissions.Prefixes.Scope + Scopes.OfflineAccess,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Phone,
                        Permissions.Scopes.Roles,
                        Permissions.Scopes.Profile,

                        Permissions.Prefixes.Scope + "api-gateway.all",

                        Permissions.Prefixes.Scope + "identity.all",
                        Permissions.Prefixes.Scope + "notification.all",
                        Permissions.Prefixes.Scope + "file-storage.all",
                    },
                    Requirements = {
                        Requirements.Features.ProofKeyForCodeExchange
                    },
                });
            }

            if (await manager.FindByClientIdAsync("mobile") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ApplicationType = ApplicationTypes.Native,
                    ClientType = ClientTypes.Public,
                    ClientId = "mobile",
                    DisplayName = "Mobile",
                    ConsentType = ConsentTypes.Explicit,
                    RedirectUris = {
                        new Uri("http://localhost:11800/signin-callback"),
                    },
                    PostLogoutRedirectUris = {
                        new Uri("http://localhost:11800"),
                    },
                    Permissions = {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Revocation,
                        Permissions.Endpoints.Logout,

                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,

                        Permissions.ResponseTypes.Code,

                        Permissions.Prefixes.Scope + Scopes.OpenId,
                        Permissions.Prefixes.Scope + Scopes.OfflineAccess,

                        Permissions.Scopes.Email,
                        Permissions.Scopes.Phone,
                        Permissions.Scopes.Roles,
                        Permissions.Scopes.Profile,

                        Permissions.Prefixes.Scope + "api-gateway.all",

                        Permissions.Prefixes.Scope + "identity.all",
                        Permissions.Prefixes.Scope + "notification.all",
                        Permissions.Prefixes.Scope + "file-storage.all",
                    },
                    Requirements = {
                        Requirements.Features.ProofKeyForCodeExchange
                    },
                });
            }
        }

        private static async Task InitResources(IServiceProvider serviceProvider)
        {
            var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("http-aggregator") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "http-aggregator",
                    // ClientSecret = "1b75d298-a5f7-4bfb-b414-120dae6ee888",
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "Http Aggregator",
                    Permissions = {
                        Permissions.Endpoints.Introspection,

                        Permissions.Prefixes.Scope + "identity.all",
                        Permissions.Prefixes.Scope + "notification.all",
                        Permissions.Prefixes.Scope + "file-storage.all",
                    },
                    // Instead of sending a client secret, this application authenticates by
                    // generating client assertions that are signed using an ECDSA signing key.
                    //
                    // Note: while the client needs access to the private key, the server only needs
                    // to know the public key to be able to validate the client assertions it receives.
                    JsonWebKeySet = new()
                    {
                        Keys = {
                            JsonWebKeyConverter.ConvertFromECDsaSecurityKey(GetECDsaSigningKey($"""
                            -----BEGIN PUBLIC KEY-----
                            MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQA0fxY4dZTqz8Pa5Wfh2s/ij4/8566
                            IFh6cd+br4BVWZW/DkdBr8JqCIq9FUZcxwo9cagHOv2+atlHQP8I4SZEN7gBBdm5
                            WoPk5W+O/nCJ4gT4cW+m/Tx5DAfqAEc4j2IdPmapeyQ49yJrQUyZoe7cYXxfOODT
                            fnNDkaSHJgh9Pv2Tpn4=
                            -----END PUBLIC KEY-----
                            """))
                        }
                    }
                });
            }

            if (await manager.FindByClientIdAsync("api-gateway") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "api-gateway",
                    ClientSecret = "1c75d298-a5f7-4bfb-b414-120dae6ee888",
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "Api Gateway Api",
                    Permissions = {
                        Permissions.GrantTypes.ClientCredentials,
                    },
                });
            }

            if (await manager.FindByClientIdAsync("identity") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "identity",
                    ClientSecret = "1d75d298-a5f7-4bfb-b414-120dae6ee888",
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "Identity Api",
                    Permissions = {
                        Permissions.Endpoints.Introspection
                    },
                });
            }

            if (await manager.FindByClientIdAsync("notification") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "notification",
                    ClientSecret = "1e75d298-a5f7-4bfb-b414-120dae6ee888",
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "Notification Api",
                    Permissions = {
                        Permissions.Endpoints.Introspection
                    },
                });
            }

            if (await manager.FindByClientIdAsync("file-storage") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "file-storage",
                    ClientSecret = "1f75d298-a5f7-4bfb-b414-120dae6ee888",
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "File Storage Api",
                    Permissions = {
                        Permissions.Endpoints.Introspection
                    },
                });
            }
        }

        private static async Task InitScopes(IServiceProvider serviceProvider)
        {
            var manager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("api-gateway.all") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor()
                {
                    Name = "api-gateway.all",
                    DisplayName = "Fully permission on Api-gateway Api",
                    Resources = {
                        "api-gateway"
                    },
                });
            }

            if (await manager.FindByNameAsync("identity.all") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor()
                {
                    Name = "identity.all",
                    DisplayName = "Fully permission on Identity Api",
                    Resources = {
                        "identity"
                    },
                });
            }

            if (await manager.FindByNameAsync("notification.all") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor()
                {
                    Name = "notification.all",
                    DisplayName = "Fully permission on Notification Api",
                    Resources = {
                        "notification"
                    },
                });
            }

            if (await manager.FindByNameAsync("file-storage.all") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor()
                {
                    Name = "file-storage.all",
                    DisplayName = "Fully permission on File Storage Api",
                    Resources = {
                        "file-storage"
                    },
                });
            }
        }

        internal class RoleInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Predefined { get; set; }
            public bool Enabled { get; set; }

            public RoleInfo(string name, string description, bool predefined, bool enabled = true)
            {
                Name = name;
                Description = description;
                Predefined = predefined;
                Enabled = enabled;
            }
        }

        internal class UserInfo
        {
            public string UserName { get; set; }
            public string RoleName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }

            public UserInfo(
                string userName, string password, string roleName,
                string firstName, string lastName,
                string email, string phoneNumber
                )
            {
                UserName = userName;
                Password = password;
                RoleName = roleName;

                FirstName = firstName;
                LastName = lastName;

                Email = email;
                PhoneNumber = phoneNumber;
            }
        }

        static ECDsaSecurityKey GetECDsaSigningKey(ReadOnlySpan<char> key)
        {
            var algorithm = ECDsa.Create();
            algorithm.ImportFromPem(key);

            return new ECDsaSecurityKey(algorithm);
        }
    }
}
