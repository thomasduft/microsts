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
  }
}