using System.Collections.Generic;

namespace tomware.STS.Web
{
  public class IdentityConfiguration
  {
    public List<UserConfiguration> Users { get; set; }
  }

  public class UserConfiguration
  {
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}