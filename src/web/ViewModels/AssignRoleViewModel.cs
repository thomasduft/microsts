using System.ComponentModel.DataAnnotations;

namespace tomware.STS.Web
{
  public class AssignViewModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string RoleName { get; set; }
  }
}
