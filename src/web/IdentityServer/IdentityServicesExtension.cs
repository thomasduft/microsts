using Microsoft.Extensions.DependencyInjection;

namespace tomware.Microsts.Web
{
  public static class IdentityServicesExtension
  {
    public static IServiceCollection AddIdentityServerServices(
      this IServiceCollection services
    )
    {
      // own services
      services.AddTransient<IClaimTypeService, ClaimTypeService>();
      services.AddTransient<IClientService, ClientService>();
      services.AddTransient<IResourceService, ResourceService>();
      services.AddTransient<IScopeService, ScopeService>();

      return services;
    }
  }
}
