using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace tomware.Microsts.Web
{
  public class AssignClaimsViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public IList<Claim> Claims { get; set; } = new List<Claim>();
  }
}
