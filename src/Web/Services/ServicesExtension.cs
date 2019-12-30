using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace tomware.STS.Web
{
  public static class ServicesExtension
  {
    public static IServiceCollection AddSTSServices(
      this IServiceCollection services,
      string authority,
      X509Certificate2 cert = null
    )
    {
      services.AddScoped<IMigrationService, MigrationService>();
      services.AddTransient<IAccountService, AccountService>();

      // Identity
      services
       .AddIdentity<ApplicationUser, IdentityRole>(o =>
       {
         o.User.RequireUniqueEmail = true;
       })
       .AddEntityFrameworkStores<STSContext>()
       .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        // Password settings.
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 5;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
      });

      // IdentityServer
      var builder = services
             .AddIdentityServer()
             .AddInMemoryIdentityResources(Config.GetIdentityResources())
             .AddInMemoryApiResources(Config.GetApiResources())
             .AddInMemoryClients(Config.GetClients(authority))
             .AddAspNetIdentity<ApplicationUser>()
             .AddProfileService<ProfileService>();

      if (cert == null)
      {
        builder.AddDeveloperSigningCredential();
      }
      else
      {
        builder.AddSigningCredential(cert);
      }

      services
        .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {
          o.Authority = authority;
          o.Audience = Constants.API_SCOPE_NAME;
        });

      services
        .AddAuthorization(options =>
        {
          options.AddPolicy(
            Policies.ADMIN_POLICY,
            policy => policy.RequireRole(Roles.ADMINISTRATOR_ROLE)
          );
        });

      return services;
    }
  }
}
