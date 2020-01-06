using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class AssignViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string RoleName { get; set; }
  }
}
