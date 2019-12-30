using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace tomware.STS.Web
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
            .WithOrigins("http://localhost:4200")
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

      // STS Services
      services.Configure<IdentityConfiguration>(
        this.Configuration.GetSection("Identities")
      );

      var authority = this.GetAuthority();
      // var cert = Program.GetCertificate(this.Configuration);
      // services.AddIdentityServices(authority, cert);
      services.AddSTSServices(authority);

      // Swagger
      services.AddSwaggerDocumentation();

      // Allow razor pages
      services.AddControllers()
        .AddNewtonsoftJson()
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
      services.AddRazorPages()
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseSerilogRequestLogging();

      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
      });
    }

    private string GetAuthority()
    {
      var domainSettings = this.Configuration.GetSection("DomainSettings");
      string schema = domainSettings.GetValue<string>("schema");
      int port = domainSettings.GetValue<int>("port");

      return $"{schema}://localhost:{port}";
    }
  }
}
