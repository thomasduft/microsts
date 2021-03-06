using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace tomware.Microsts.Web
{
  public static class ServicesExtension
  {
    public static IServiceCollection AddSTSServices(
      this IServiceCollection services
    )
    {
      var configuration = services
        .BuildServiceProvider()
        .GetRequiredService<IConfiguration>();

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

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
      });

      // IdentityServer
      var builder = services
        .AddIdentityServer(options =>
        {
          options.Events.RaiseErrorEvents = true;
          options.Events.RaiseInformationEvents = true;
          options.Events.RaiseFailureEvents = true;
          options.Events.RaiseSuccessEvents = true;
          options.UserInteraction.LoginUrl = "/Identity/Account/Login";
          options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
          options.Authentication = new AuthenticationOptions()
          {
            CookieLifetime = TimeSpan.FromHours(8), // ID server cookie timeout set to 10 hours
            CookieSlidingExpiration = true
          };
        })
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryPersistedGrants()
        // .AddInMemoryCaching()
        // .AddOperationalStore()
        .AddConfigurationStore(options =>
        {
          options.ConfigureDbContext = builder =>
              builder.UseSqlite(configuration["ConnectionString"],
                  sql => sql.MigrationsAssembly(typeof(ServicesExtension)
                    .GetTypeInfo()
                    .Assembly
                    .GetName()
                    .Name));
        })
        .AddAspNetIdentity<ApplicationUser>()
        .AddProfileService<ProfileService>();

      X509Certificate2 cert = GetCertificate(configuration);
      if (cert == null)
      {
        builder.AddDeveloperSigningCredential(persistKey: false);
      }
      else
      {
        builder.AddSigningCredential(cert);
      }

      var authority = GetAuthority(configuration);
      services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        // .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, opt =>
        // {
        //   opt.Authority = authority;
        //   opt.ApiName = Constants.STS_API;
        //   opt.RequireHttpsMetadata = false;
        // })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
          opt.Authority = authority;
          opt.Audience = Constants.STS_API;
          opt.RequireHttpsMetadata = false;
          opt.IncludeErrorDetails = true;
          opt.SaveToken = true;
          opt.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidateAudience = false,
            NameClaimType = "name",
          };
        })
        .AddLocalApi(options =>
        {
          options.ExpectedScope = Constants.STS_API;
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

      // own services
      services.AddScoped<IMigrationService, MigrationService>();
      services.AddTransient<IEmailSender, LogEmailSender>();
      services.AddSingleton<ITitleService, TitleService>();

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
