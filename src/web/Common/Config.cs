using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace tomware.Microsts.Web
{
  public class Config
  {
    public static IEnumerable<IdentityServer4.Models.IdentityResource> GetIdentityResources()
    {
      return new List<IdentityServer4.Models.IdentityResource>
      {
        new IdentityServer4.Models.IdentityResources.OpenId(),
        new IdentityServer4.Models.IdentityResources.Profile()
      };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
      return new List<ApiScope>
      {
         new ApiScope(Constants.STS_API, Constants.STS_API_DISPLAY_NAME)
      };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      return new List<ApiResource>
      {
        new ApiResource(Constants.STS_API, Constants.STS_API_DISPLAY_NAME)
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
          ClientUri = authority,

          AllowedGrantTypes = GrantTypes.Code,
          RequirePkce = true,
          RequireClientSecret = false,

          AllowAccessTokensViaBrowser = true,
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
            Constants.STS_API
          }
        }
      };
    }
  }
}