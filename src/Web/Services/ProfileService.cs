using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace tomware.STS.Web
{
  public class ProfileService : IProfileService
  {
    private readonly UserManager<ApplicationUser> userManager;

    public ProfileService(
      UserManager<ApplicationUser> userManager
    )
    {
      this.userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      var sub = context.Subject?.GetSubjectId();
      if (sub == null) throw new Exception("No sub claim present");

      var user = await this.userManager.FindByIdAsync(sub);
      if (user != null)
      {
        var claims = new List<Claim>
        {
          new Claim(JwtClaimTypes.Subject, user.Id),
          new Claim(JwtClaimTypes.Name, user.UserName),
          new Claim(JwtClaimTypes.Email, user.Email)
        };

        var roles = await this.userManager.GetRolesAsync(user);
        if (roles.Count > 0)
        {
          claims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r)));
        }

        context.AddRequestedClaims(claims);
      }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
      var sub = context.Subject?.GetSubjectId();
      if (sub == null) throw new Exception("No subject Id claim present");

      var user = await this.userManager.FindByIdAsync(sub);

      context.IsActive = user != null;
    }
  }
}