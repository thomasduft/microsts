using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class AddRoleViewModel
  {
    [Required]
    public string RoleName { get; set; }
  }
}
