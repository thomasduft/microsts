using System.ComponentModel.DataAnnotations;

namespace tomware.STS.Web
{
  public class AddRoleViewModel
  {
    [Required]
    public string RoleName { get; set; }
  }
}
