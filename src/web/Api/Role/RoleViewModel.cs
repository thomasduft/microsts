using System.ComponentModel.DataAnnotations;

namespace tomware.Microsts.Web
{
  public class RoleViewModel
  {
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
  }
}
