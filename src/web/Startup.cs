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

      services.Configure<CookiePolicyOptions>(options =>
      {
        options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
        options.OnAppendCookie = cookieContext =>
          CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        options.OnDeleteCookie = cookieContext =>
          CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
      });

      var cookieSecurePolicy = CookieSecurePolicy.SameAsRequest; // CookieSecurePolicy.Always;
      services.AddAntiforgery(options =>
      {
        options.SuppressXFrameOptionsHeader = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = cookieSecurePolicy;
      });

      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromMinutes(2);
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = cookieSecurePolicy;
      });

      var connection = this.Configuration["ConnectionString"];
      services.AddDbContext<STSContext>(o => o.UseSqlite(connection));

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

      // Api Services
      services.AddApiServices();
      
      // STS Services
      services.AddSTSServices();

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

    private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
    {
      if (options.SameSite == SameSiteMode.None)
      {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        if (DisallowsSameSiteNone(userAgent))
        {
          // For .NET Core < 3.1 set SameSite = (SameSiteMode)(-1)
          options.SameSite = SameSiteMode.Unspecified;
        }
      }
    }

    private static bool DisallowsSameSiteNone(string userAgent)
    {
      // Cover all iOS based browsers here. This includes:
      // - Safari on iOS 12 for iPhone, iPod Touch, iPad
      // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
      // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
      // All of which are broken by SameSite=None, because they use the iOS networking stack
      if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
      {
        return true;
      }

      // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
      // - Safari on Mac OS X.
      // This does not include:
      // - Chrome on Mac OS X
      // Because they do not use the Mac OS networking stack.
      if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
          userAgent.Contains("Version/") && userAgent.Contains("Safari"))
      {
        return true;
      }

      // Cover Chrome 50-69, because some versions are broken by SameSite=None,
      // and none in this range require it.
      // Note: this covers some pre-Chromium Edge versions,
      // but pre-Chromium Edge does not require SameSite=None.
      if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
      {
        return true;
      }

      return false;
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
