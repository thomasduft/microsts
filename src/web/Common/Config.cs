using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace tomware.Microsts.Web
{
  public class Config
  {
    public static IEnumerable<IdentityResource> GetIds()
    {
      return new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
      };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
      {
        new ApiResource("sts_api", "STS API")
      };
    }

    public static IEnumerable<Client> GetClients(string authority)
    {
      return new List<Client>
      {
        new Client
        {
          ClientId = "stsclient",
          ClientName = "STS admin client",
          RequireClientSecret = false,
          RequirePkce = true,
          RequireConsent = false,
          AllowAccessTokensViaBrowser = true,
          AllowedGrantTypes = GrantTypes.Code,
          AllowOfflineAccess = true,
          RedirectUris = {
            authority,
            "http://localhost:4200"
          },
          PostLogoutRedirectUris = {
            authority,
            "http://localhost:4200"
          },
          AllowedCorsOrigins = {
            authority,
            "http://localhost:4200"
          },
          AllowedScopes = {
            IdentityServerConstants.StandardScopes.OpenId,
            "sts_api"
          }
        }
      };
    }
  }
}