using System.Collections.Generic;

namespace tomware.Microsts.Web
{
  // TODO: better name for ClientSettings
  public class ClientSettings
  {
    public List<ClientConfiguration> Clients { get; set; }
  }

  public class ClientConfiguration
  {
    public string ClientId { get; set; }
    public string Issuer { get; set; }
    public string RedirectUri { get; set; }
    public string Scope { get; set; }
    public string LoginUrl { get; set; }
    public string LogoutUrl { get; set; }
  }
}