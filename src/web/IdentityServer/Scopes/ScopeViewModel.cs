using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class ScopeViewModel
  {
    [Required]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public bool Required { get; set; } = false;

    public List<string> UserClaims { get; set; }
  }
}
