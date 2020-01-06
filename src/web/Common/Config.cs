using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace tomware.Microsts.Web
{
  public class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
      };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      return new List<ApiResource>
      {
        new ApiResource(Constants.API_SCOPE_NAME, "STS API") {
          UserClaims = {
            JwtClaimTypes.Subject,
            JwtClaimTypes.Name,
            JwtClaimTypes.Role
          }
        }
      };
    }

    public static IEnumerable<Client> GetClients(string authority)
    {
      // client credentials client
      return new List<Client>
      {
        new Client
        {
          ClientId = "spaclient",
          ClientName = "spaclient",
          AccessTokenType = AccessTokenType.Jwt,
          AllowedGrantTypes = GrantTypes.Implicit,
          RequireConsent = false,
          AllowAccessTokensViaBrowser = true,
          RedirectUris = new List<string>
          {
            authority,
            "http://localhost:4200"
          },
          PostLogoutRedirectUris = new List<string>
          {
            authority,
            "http://localhost:4200"
          },
          AllowedCorsOrigins =
          {
            authority,
            "http://localhost:4200"
          },
          AllowedScopes = {
            IdentityServerConstants.StandardScopes.OpenId, // For UserInfo endpoint.
            IdentityServerConstants.StandardScopes.Profile,
            Constants.API_SCOPE_NAME
          }
        }
      };
    }
  }
}