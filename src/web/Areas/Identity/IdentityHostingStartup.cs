using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(tomware.Microsts.Web.Areas.Identity.IdentityHostingStartup))]
namespace tomware.Microsts.Web.Areas.Identity
{
  public class IdentityHostingStartup : IHostingStartup
  {
    public void Configure(IWebHostBuilder builder)
    {
      builder.ConfigureServices((context, services) =>
      {
      });
    }
  }
}