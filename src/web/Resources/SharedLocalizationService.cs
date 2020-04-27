using Microsoft.Extensions.Localization;
using System.Reflection;

namespace tomware.Microsts.Web.Resources
{
  public class SharedLocalizationService
  {
    private readonly IStringLocalizer localizer;

    public SharedLocalizationService(IStringLocalizerFactory factory)
    {
      var type = typeof(SharedResource);
      var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
      
      localizer = factory.Create(nameof(IdentityResource), assemblyName.Name);
    }

    public LocalizedString GetLocalizedHtmlString(string key)
    {
      return localizer[key];
    }
  }
}