using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace IdentityServer.DataContexts;

public static class IdentityServerDbSeeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        if (!context.Clients.Any())
            SeedClients(context);

        if (!context.IdentityResources.Any())
            SeedIdentityResources(context);

        if (!context.ApiScopes.Any())
            SeedApiScopes(context);

        if (!context.ApiResources.Any())
            SeedApiResources(context);
    }

    private static void SeedClients(ConfigurationDbContext context)
    {
        var client1 = new Client
        {
            ClientId = "movieClient",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
            AllowedScopes = { "movieAPI" }
        };

        var client2 = new Client
        {
            ClientId = "movies_mvc_client",
            ClientName = "Movies MVC Web App",
            AllowedGrantTypes = GrantTypes.Hybrid,
            RequirePkce = false,
            AllowRememberConsent = false,
            RedirectUris = new List<string>()
                       {
                           "https://localhost:5002/signin-oidc"
                       },
            PostLogoutRedirectUris = new List<string>()
                       {
                           "https://localhost:5002/signout-callback-oidc"
                       },
            ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
            AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                           IdentityServerConstants.StandardScopes.Address,
                           IdentityServerConstants.StandardScopes.Email,
                           "movieAPI",
                           "roles"
                       }
        };

        context.Clients.AddRange(client1.ToEntity(), client2.ToEntity());
        context.SaveChanges();
    }

    private static void SeedIdentityResources(ConfigurationDbContext context)
    {
        var identityResources = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource("roles", "Your role(s)", new List<string> { "role" }),
            new IdentityResource("email", "Your email address", new List<string> { "email" })
        };
        context.IdentityResources.AddRange(identityResources.Select(x => x.ToEntity()));
        context.SaveChanges();
    }

    private static void SeedTestUsers(ConfigurationDbContext context)
    {
        var users = new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "93F4C4CF-F681-43A7-A821-EA17341CCFCB",
                Username = "sam",
                Password = "abc",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "sam"),
                    new Claim(JwtClaimTypes.FamilyName, "henry")
                }
            }
        };
    }

    private static void SeedApiScopes(ConfigurationDbContext context)
    {
        var scopes = new List<ApiScope>
        {
            new ApiScope("movieAPI", "Movie API")
        };

        foreach (var scope in scopes)
        {
            context.ApiScopes.Add(scope.ToEntity());
        }

        context.SaveChanges();
    }

    private static void SeedApiResources(ConfigurationDbContext context)
    {
        var apiResource = new ApiResource("movieAPI", "Movie API")
        {
            Scopes = { "movieAPI" }
        };

        context.ApiResources.Add(apiResource.ToEntity());
        context.SaveChanges();
    }
}