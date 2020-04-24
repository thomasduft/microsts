using Microsoft.Extensions.Localization;
using System.Reflection;

namespace tomware.Microsts.Web
{
  public class SharedLocalizationService
  {
    private readonly IStringLocalizer localizer;

    public SharedLocalizationService(IStringLocalizerFactory factory)
    {
      var type = typeof(IdentityResource);
      var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
      
      localizer = factory.Create("SharedResource", assemblyName.Name);
    }

    public LocalizedString GetLocalizedHtmlString(string key)
    {
      return localizer[key];
    }
  }
}