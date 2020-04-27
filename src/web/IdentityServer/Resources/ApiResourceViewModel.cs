using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class ApiResourceViewModel
  {
    public int? Id { get; set; }
    
    public bool Enabled { get; set; }

    [Required]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<string> UserClaims { get; set; } = new List<string>();

    public List<string> Scopes { get; set; } = new List<string>();
  }
}
