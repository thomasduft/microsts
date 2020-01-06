using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tomware.Microsts.Web
{
  public interface IMigrationService
  {
    Task EnsureMigrationAsync();
  }

  public class MigrationService : IMigrationService
  {
    private readonly STSContext context;
    private readonly IdentityConfiguration config;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public MigrationService(
      STSContext context,
      IOptions<IdentityConfiguration> config,
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager
      )
    {
      this.context = context;
      this.config = config.Value;
      this.userManager = userManager;
      this.roleManager = roleManager;
    }

    public async Task EnsureMigrationAsync()
    {
      await this.context.Database.MigrateAsync();

      // Ensure roles
      await this.EnsureRole(Roles.ADMINISTRATOR_ROLE);

      // Ensure users
      await this.EnsureUser(Constants.ADMIN_USER, new List<string>
      {
        Roles.ADMINISTRATOR_ROLE,
      });
    }

    private async Task EnsureRole(string role)
    {
      var roleExists = await this.roleManager.RoleExistsAsync(role);
      if (!roleExists)
      {
        var newRole = new IdentityRole(role);
        await this.roleManager.CreateAsync(newRole);
      }
    }

    private async Task EnsureUser(
        string userName,
        IEnumerable<string> roles
    )
    {
      var userConfig = this.config.Users.FirstOrDefault(u => u.UserName == userName);
      if (userConfig == null) return;

      var user = await this.userManager.FindByNameAsync(userConfig.UserName);
      if (user != null) return;

      var applicationUser = new ApplicationUser
      {
        UserName = userConfig.UserName,
        Email = userConfig.Email
      };

      var userResult = await this.userManager.CreateAsync(applicationUser, userConfig.Password);
      if (!userResult.Succeeded) return;

      await this.userManager.SetLockoutEnabledAsync(applicationUser, false);

      foreach (var role in roles)
      {
        await this.userManager.AddToRoleAsync(applicationUser, role);
      }
    }
  }
}