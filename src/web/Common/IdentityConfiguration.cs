using System.Collections.Generic;

namespace tomware.STS.Web
{
  public class IdentityConfiguration
  {
    public List<UserConfig> Users { get; set; }
  }

  public class UserConfig
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}