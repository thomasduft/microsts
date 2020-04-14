using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace tomware.Microsts.Web
{
  public class Config
  {
    public static IEnumerable<IdentityResource> Ids =>
      new List<IdentityResource>
        {
          new IdentityResources.OpenId(),
          new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> Apis =>
      new List<ApiResource>
        {
          new ApiResource("sts_api", "STS API")
        };

    public static IEnumerable<Client> Clients =>
      new List<Client>
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
            "http://localhost:5000",
            "http://localhost:4200"
          },
          PostLogoutRedirectUris = {
            "http://localhost:5000",
            "http://localhost:4200"
          },
          AllowedCorsOrigins = {
            "http://localhost:5000",
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