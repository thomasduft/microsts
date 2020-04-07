using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class ClientViewModel
  {
    [Required]
    public bool Enabled { get; set; }

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientName { get; set; }

    public bool RequireClientSecret { get; set; }

    public bool RequirePkce { get; set; }

    public bool RequireConsent { get; set; }

    public bool AllowAccessTokensViaBrowser { get; set; }

    public List<string> AllowedGrantTypes { get; set; } = new List<string>();

    public List<string> RedirectUris { get; set; } = new List<string>();

    public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();

    public List<string> AllowedCorsOrigins { get; set; } = new List<string>();

    public List<string> AllowedScopes { get; set; } = new List<string>();
  }
}
