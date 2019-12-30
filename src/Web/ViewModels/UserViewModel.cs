using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace tomware.STS.Web
{
  public class UserViewModel
  {
    [Required]
    public string Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    public bool LockoutEnabled { get; set; }

    public List<Claim> Claims { get; set; }

    public List<string> Roles { get; set; }
  }
}
