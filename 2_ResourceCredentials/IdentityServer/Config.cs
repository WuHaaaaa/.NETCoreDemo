using System.Collections;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("orderApi", "Order Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "orderApi" }
                },
                new Client()
                {
                    ClientId = "WinForm",
                    ClientSecrets =
                    {
                        new Secret("winform_secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {"orderApi"},
                    AllowOfflineAccess = true, //允许返回Refresh Token
                }
            };
        }

        public static List<TestUser> GeTestUsers()
        {
            return  new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "Zoe",
                    Password = "123456"
                },
                new TestUser()
                {
                    SubjectId = "2",
                    Username = "Ran",
                    Password = "111222"
                }
            };
        }
    };
}