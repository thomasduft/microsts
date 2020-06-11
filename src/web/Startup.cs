using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using tomware.Microsts.Web.Resources;

namespace tomware.Microsts.Web
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(o =>
      {
        o.AddPolicy("AllowAllOrigins", builder =>
        {
          builder
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-Pagination");
        });
      });

      services.AddRouting(o => o.LowercaseUrls = true);

      var connection = this.Configuration["ConnectionString"];
      services
        .AddEntityFrameworkSqlite()
        .AddDbContext<STSContext>(o => o.UseSqlite(connection));

      // localization
      services.AddSingleton<IdentityLocalizationService>();
      services.AddSingleton<SharedLocalizationService>();

      services.AddLocalization(options => options.ResourcesPath = "Resources");
      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new List<CultureInfo>
          {
          new CultureInfo("en-US"),
          new CultureInfo("de-CH")
        };
        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
      });

      // STS Services
      services.AddSTSServices();
      services.AddIdentityServerServices();

      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromMinutes(2);
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // TODO: check https or not
      });

      // Swagger
      services.AddSwaggerDocumentation();

      // Allow razor pages
      services.AddControllers()
        .AddNewtonsoftJson()
        .SetCompatibilityVersion(CompatibilityVersion.Latest);

      var builder = services.AddRazorPages()
        .SetCompatibilityVersion(CompatibilityVersion.Latest)
        .AddViewLocalization()
        .AddDataAnnotationsLocalization(options =>
        {
          options.DataAnnotationLocalizerProvider = (type, factory) =>
          {
            var name = new AssemblyName(typeof(IdentityResource).GetTypeInfo().Assembly.FullName);
            return factory.Create(nameof(IdentityResource), name.Name);
          };
        });

      var allowSelfRegister = this.Configuration.GetValue<bool>("AllowSelfRegister");
      if (!allowSelfRegister)
      {
        builder.AddRazorPagesOptions(options =>
        {
          options.Conventions.AuthorizeAreaPage(
            "identity",
            "/account/register",
            Policies.ADMIN_POLICY
          );
        });
      }
    }

    public void Configure(
      IApplicationBuilder app,
      IWebHostEnvironment env
    )
    {
      if (env.IsDevelopment())
      {
        app.UseCors("AllowAllOrigins");
        app.UseSwaggerDocumentation();

        app.UseDeveloperExceptionPage();
      }

      app.UseMiddleware<SecurityHeadersMiddleware>();

      ConsiderSpaRoutes(app);

      var locOptions = app.ApplicationServices
        .GetService<IOptions<RequestLocalizationOptions>>();
      app.UseRequestLocalization(locOptions.Value);

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseSerilogRequestLogging();

      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapDefaultControllerRoute();
      });
    }

    private static void ConsiderSpaRoutes(IApplicationBuilder app)
    {
      var angularRoutes = new[]
      {
        "/home",
        "/forbidden",
        "/claimtypes",
        "/roles",
        "/users",
        "/users/register",
        "/resources",
        "/clients"
      };

      app.Use(async (context, next) =>
      {
        if (context.Request.Path.HasValue
          && null != angularRoutes.FirstOrDefault(
            (ar) => context.Request.Path.Value.StartsWith(ar, StringComparison.OrdinalIgnoreCase)))
        {
          context.Request.Path = new PathString("/");
        }

        await next();
      });
    }
  }
}
