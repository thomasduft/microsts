using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class AssignClaimsViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public IList<string> Claims { get; set; } = new List<string>();
  }
}
