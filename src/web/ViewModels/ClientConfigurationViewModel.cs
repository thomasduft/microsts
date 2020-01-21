namespace tomware.Microsts.Web
{
  public class ClientConfigurationViewModel
  {
    public string ClientId { get; set; }
    public string Issuer { get; set; }
    public string RedirectUri { get; set; }
    public string ResponseType { get; set; }
    public string Scope { get; set; }
    public string LoginUrl { get; set; }
    public string LogoutUrl { get; set; }
  }
}