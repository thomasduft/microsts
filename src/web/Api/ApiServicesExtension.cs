using Microsoft.Extensions.DependencyInjection;

namespace tomware.Microsts.Web
{
  public static class ApiServicesExtension
  {
    public static IServiceCollection AddApiServices(
      this IServiceCollection services
    )
    {
      services.AddTransient<IAccountService, AccountService>();
      services.AddTransient<IClaimTypeService, ClaimTypeService>();
      services.AddTransient<IClientService, ClientService>();
      services.AddTransient<IResourceService, ResourceService>();
      services.AddTransient<IRoleService, RoleService>();
      services.AddTransient<IScopeService, ScopeService>();

      return services;
    }
  }
}
