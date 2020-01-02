using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

namespace tomware.STS.Web
{
  public static class ServicesExtension
  {
    public static IServiceCollection AddSTSServices(
      this IServiceCollection services
    )
    {
      services.AddScoped<IMigrationService, MigrationService>();
      services.AddTransient<IAccountService, AccountService>();
      services.AddTransient<IClientConfigurationService, ClientConfigurationService>();

      var configuration = services
        .BuildServiceProvider()
        .GetRequiredService<IConfiguration>();

      // Identity
      services
       .AddIdentity<ApplicationUser, IdentityRole>(o =>
       {
         o.User.RequireUniqueEmail = true;
       })
       .AddEntityFrameworkStores<STSContext>();
      // .AddDefaultTokenProviders();

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
        .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
        .AddAspNetIdentity<ApplicationUser>()
        .AddProfileService<ProfileService>();

      X509Certificate2 cert = GetCertificate(configuration);
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
        .AddIdentityServerAuthentication(o =>
        {
          o.Authority = GetAuthority(configuration);
          o.ApiName = Constants.API_SCOPE_NAME;
          o.SupportedTokens = SupportedTokens.Both;
        });

      services
        .AddAuthorization(options =>
        {
          options.AddPolicy(
            Policies.ADMIN_POLICY,
            policy => policy
              .RequireAuthenticatedUser()
              .RequireRole(Roles.ADMINISTRATOR_ROLE)
          );
        });

      return services;
    }

    private static string GetAuthority(IConfiguration configuration)
    {
      return Program.GetUrls(configuration);
    }

    private static X509Certificate2 GetCertificate(IConfiguration config)
    {
      var settings = config.GetSection("CertificateSettings");
      if (settings == null) return null;

      string filename = settings.GetValue<string>("filename");
      string password = settings.GetValue<string>("password");
      if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(password))
      {
        return new X509Certificate2(filename, password);
      }

      return null;
    }
  }
}
