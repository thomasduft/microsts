using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace tomware.Microsts.Web
{
  public interface IAccountService
  {
    ApplicationUser CreateUser(RegisterUserViewModel model);
    Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
    Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model);
    Task<IEnumerable<UserViewModel>> GetUsersAsync();
    Task<UserViewModel> GetUserAsync(string id);
    Task<IdentityResult> AssignClaimsAsync(AssignClaimsViewModel model);
    Task<IdentityResult> AssignRolesAsync(AssignRolesViewModel model);
  }

  public class AccountService : IAccountService
  {
    private readonly UserManager<ApplicationUser> userManager;

    public AccountService(
      UserManager<ApplicationUser> userManager
    )
    {
      this.userManager = userManager;
    }

    public ApplicationUser CreateUser(RegisterUserViewModel model)
    {
      return new ApplicationUser
      {
        UserName = model.UserName,
        Email = model.Email
      };
    }

    public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
    {
      return await this.userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model)
    {
      var user = await this.userManager.FindByNameAsync(model.UserName);

      return await this.userManager.ChangePasswordAsync(
        user,
        model.CurrentPassword,
        model.NewPassword
      );
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
    {
      // TODO: Paging
      var items = await this.userManager.Users
        .OrderBy(u => u.UserName)
        .AsNoTracking()
        .ToListAsync();

      return items.Select(u => new UserViewModel
      {
        Id = u.Id,
        UserName = u.UserName,
        Email = u.Email,
        LockoutEnabled = u.LockoutEnabled
      });
    }

    public async Task<UserViewModel> GetUserAsync(string id)
    {
      var user = await this.userManager.FindByIdAsync(id);
      var roles = await this.userManager.GetRolesAsync(user);
      var claims = await this.userManager.GetClaimsAsync(user);

      return new UserViewModel
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        LockoutEnabled = user.LockoutEnabled,
        Claims = claims.ToList(),
        Roles = roles.ToList()
      };
    }

    public async Task<IdentityResult> AssignClaimsAsync(AssignClaimsViewModel model)
    {
      if (model is null) throw new ArgumentNullException(nameof(model));

      var user = await this.userManager.FindByNameAsync(model.UserName);
      var claims = await this.userManager.GetClaimsAsync(user);

      // removing all claims
      await this.userManager.RemoveClaimsAsync(user, claims.Where(c => c.Type == "tw"));

      // assigning claims
      return await this.userManager.AddClaimsAsync(
        user,
        model.Claims.Select(c => new Claim("tw", c))
      );
    }

    public async Task<IdentityResult> AssignRolesAsync(AssignRolesViewModel model)
    {
      if (model is null) throw new ArgumentNullException(nameof(model));

      var user = await this.userManager.FindByNameAsync(model.UserName);
      var roles = await this.userManager.GetRolesAsync(user);

      // removing all roles
      await this.userManager.RemoveFromRolesAsync(user, roles);

      // assigning roles
      return await this.userManager.AddToRolesAsync(
        user,
        model.Roles
      );
    }
  }
}
