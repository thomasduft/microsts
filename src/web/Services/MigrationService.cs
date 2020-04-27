using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    private readonly ConfigurationDbContext configurationDbContext;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration config;

    public MigrationService(
      STSContext context,
      ConfigurationDbContext configurationDbContext,
      UserManager<ApplicationUser> userManager,
      RoleManager<IdentityRole> roleManager,
      IConfiguration config
      )
    {
      this.context = context;
      this.configurationDbContext = configurationDbContext;
      this.userManager = userManager;
      this.roleManager = roleManager;
      this.config = config;
    }

    public async Task EnsureMigrationAsync()
    {
      await this.context.Database.MigrateAsync();

      await this.EnsureAdministratorRole();
      await this.EnsureAdministratorUser();

      // IdentityServer
      await this.configurationDbContext.Database.MigrateAsync();

      var authority = !string.IsNullOrWhiteSpace(this.config["AuthorityForDocker"])
        ? this.config["AuthorityForDocker"]
        : Program.GetUrls(this.config);
      await this.SeedDefaultConfiguration(
        this.configurationDbContext,
        authority
      );
    }

    private async Task EnsureAdministratorRole()
    {
      var role = Roles.ADMINISTRATOR_ROLE;
      var roleExists = await this.roleManager.RoleExistsAsync(role);
      if (!roleExists)
      {
        var newRole = new IdentityRole(role);
        await this.roleManager.CreateAsync(newRole);
      }
    }

    private async Task EnsureAdministratorUser()
    {
      var user = await this.userManager.FindByNameAsync(Constants.ADMIN_USER);
      if (user != null) return;

      var applicationUser = new ApplicationUser
      {
        UserName = Constants.ADMIN_USER,
        Email = "admin@sts.com"
      };

      var userResult = await this.userManager.CreateAsync(applicationUser, "Pass123$");
      if (!userResult.Succeeded) return;

      await this.userManager.SetLockoutEnabledAsync(applicationUser, false);
      await this.userManager.AddToRoleAsync(applicationUser, Roles.ADMINISTRATOR_ROLE);
    }

    private async Task SeedDefaultConfiguration(IConfigurationDbContext context, string authority)
    {
      if (!context.Clients.Any())
      {
        foreach (var client in Config.GetClients(authority))
        {
          context.Clients.Add(client.ToEntity());
        }

        await context.SaveChangesAsync();
      }

      if (!context.IdentityResources.Any())
      {
        foreach (var resource in Config.GetIds())
        {
          context.IdentityResources.Add(resource.ToEntity());
        }

        await context.SaveChangesAsync();
      }

      if (!context.ApiResources.Any())
      {
        foreach (var resource in Config.GetApis())
        {
          context.ApiResources.Add(resource.ToEntity());
        }

        await context.SaveChangesAsync();
      }
    }
  }
}