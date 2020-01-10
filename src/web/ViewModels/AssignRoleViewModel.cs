using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class AssignRolesViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public IList<string> Roles { get; set; } = new List<string>();
  }
}
