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
    Task<IdentityResult> UpdateAsync(UserViewModel model);
  }

  public class AccountService : IAccountService
  {
    private readonly UserManager<ApplicationUser> manager;

    public AccountService(
      UserManager<ApplicationUser> manager
    )
    {
      this.manager = manager;
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
      return await this.manager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model)
    {
      var user = await this.manager.FindByNameAsync(model.UserName);

      return await this.manager.ChangePasswordAsync(
        user,
        model.CurrentPassword,
        model.NewPassword
      );
    }

    public async Task<IEnumerable<UserViewModel>> GetUsersAsync()
    {
      // TODO: Paging
      var items = await this.manager.Users
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
      var user = await this.manager.FindByIdAsync(id);
      var roles = await this.manager.GetRolesAsync(user);
      var claims = await this.manager.GetClaimsAsync(user);

      return new UserViewModel
      {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        LockoutEnabled = user.LockoutEnabled,
        Claims = new List<string>(claims.ToList().Select(c => c.Value)),
        Roles = roles.ToList()
      };
    }

    public async Task<IdentityResult> UpdateAsync(UserViewModel model)
    {
      if (model == null) throw new ArgumentNullException(nameof(model));
      if (string.IsNullOrWhiteSpace(model.Id)) throw new ArgumentNullException(nameof(model.Id));

      var user = await this.manager.FindByIdAsync(model.Id);
      user.UserName = model.UserName;
      user.Email = model.Email;
      user.LockoutEnabled = model.LockoutEnabled;

      var result = await this.manager.UpdateAsync(user);

      result = await this.AssignClaimsAsync(user, new AssignClaimsViewModel
      {
        UserName = user.UserName,
        Claims = model.Claims
      });

      if (!result.Succeeded)
      {
        return result;
      }

      result = await this.AssignRolesAsync(user, new AssignRolesViewModel
      {
        UserName = user.UserName,
        Roles = model.Roles
      });

      return result;
    }

    private async Task<IdentityResult> AssignClaimsAsync(
      ApplicationUser user,
      AssignClaimsViewModel model
    )
    {
      var claims = await this.manager.GetClaimsAsync(user);

      // removing all claims
      await this.manager.RemoveClaimsAsync(user, claims.Where(c => c.Type == "tw"));

      // assigning claims
      return await this.manager.AddClaimsAsync(
        user,
        model.Claims.Select(c => new Claim("tw", c))
      );
    }

    private async Task<IdentityResult> AssignRolesAsync(
      ApplicationUser user,
      AssignRolesViewModel model
    )
    {
      var roles = await this.manager.GetRolesAsync(user);

      // removing all roles
      await this.manager.RemoveFromRolesAsync(user, roles);

      // assigning roles
      return await this.manager.AddToRolesAsync(
        user,
        model.Roles
      );
    }
  }
}
