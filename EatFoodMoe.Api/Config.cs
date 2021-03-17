using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EatFoodMoe.Api
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource{
                    Name = "role", DisplayName = "role",
                    UserClaims = new []
                    {
                        "role", ClaimTypes.Role
                    }
                }
            };
        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource("user_api", "User Api")
                {
                    Scopes = { "user_api" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope
                {
                    Name = "user_api"
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "WebClient",
                    ClientName = "Web Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    //ClientUri = "http://localhost:3000",

                    //AllowedCorsOrigins = { "http://localhost:3000" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "user_api",
                        "role"
                    },
                },
            };
    }
}
