using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tomware.Microsts.Web
{
  public interface IAccountService
  {
    ApplicationUser CreateUser(RegisterViewModel model);
    Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
    Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model);
    IEnumerable<string> GetRoles();
    Task<IdentityResult> AddRoleAsync(string role);
    Task<IdentityResult> AssignRoleAsync(AssignViewModel model);
    Task<IdentityResult> UnassignRoleAsync(AssignViewModel model);
    IEnumerable<UserViewModel> GetUsers();
    Task<UserViewModel> GetUser(string id);
  }

  public class AccountService : IAccountService
  {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AccountService(
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager
    )
    {
      this.userManager = userManager;
      this.roleManager = roleManager;
    }

    public ApplicationUser CreateUser(RegisterViewModel model)
    {
      return new ApplicationUser
      {
        UserName = model.Email,
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

      return await this.userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
    }

    public IEnumerable<string> GetRoles()
    {
      var roles = this.roleManager.Roles.ToList().Select(r => r.Name);

      return roles;
    }

    public async Task<IdentityResult> AddRoleAsync(string role)
    {
      var newRole = new IdentityRole(role);
      if (!await this.roleManager.RoleExistsAsync(role))
      {
        return await this.roleManager.CreateAsync(newRole);
      }

      return IdentityResult.Failed();
    }

    public async Task<IdentityResult> AssignRoleAsync(AssignViewModel model)
    {
      var role = await this.roleManager.FindByNameAsync(model.RoleName);
      if (role == null) return await this.RoleDoesNotExist(model.RoleName);

      var user = await this.userManager.FindByNameAsync(model.UserName);
      if (!await this.userManager.IsInRoleAsync(user, model.RoleName))
      {
        return await this.userManager.AddToRoleAsync(user, model.RoleName);
      }

      return IdentityResult.Failed();
    }

    public async Task<IdentityResult> UnassignRoleAsync(AssignViewModel model)
    {
      var role = this.roleManager.FindByNameAsync(model.RoleName).Result;
      if (role == null) return await this.RoleDoesNotExist(model.RoleName);

      var user = await this.userManager.FindByNameAsync(model.UserName);
      if (await this.userManager.IsInRoleAsync(user, model.RoleName))
      {
        return await this.userManager.RemoveFromRoleAsync(user, model.RoleName);
      }

      return IdentityResult.Failed();
    }

    public IEnumerable<UserViewModel> GetUsers()
    {
      var users = this.userManager.Users.ToList()
        .Select(u => new UserViewModel
        {
          Id = u.Id,
          UserName = u.UserName,
          Email = u.Email,
          LockoutEnabled = u.LockoutEnabled
        });

      return users;
    }

    public async Task<UserViewModel> GetUser(string id)
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

    private Task<IdentityResult> RoleDoesNotExist(string role)
    {
      return Task.FromResult(IdentityResult.Failed(new[]
      {
        new IdentityError
        {
          Code = "RoleDoesNotExist",
          Description = $"The role {role} does not exist! Please add it first."
        }
      }));
    }
  }
}
