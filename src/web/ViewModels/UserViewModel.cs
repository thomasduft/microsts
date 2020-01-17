using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
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

    public bool IsLockedOut { get; set; }

    public List<string> Claims { get; set; }

    public List<string> Roles { get; set; }
  }
}
