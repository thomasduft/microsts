using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace tomware.STS.Web.Pages.Account
{
  public class LogoutModel : PageModel
  {
    private readonly ILogger<LogoutModel> logger;
    private readonly SignInManager<ApplicationUser> signInManager;

    public LogoutModel(
      ILogger<LogoutModel> logger,
      SignInManager<ApplicationUser> signInManager
    )
    {
      this.logger = logger;
      this.signInManager = signInManager;
    }

    public async Task OnGet(string returnUrl = null)
    {
      await this.Logout(returnUrl);
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
      await this.Logout(returnUrl);

      if (returnUrl != null)
      {
        return this.LocalRedirect(returnUrl);
      }
      else
      {
        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return this.RedirectToPage();
      }
    }

    private async Task Logout(string returnUrl = null)
    {
      await this.signInManager.SignOutAsync();

      this.logger.LogInformation("User logged out.");
    }
  }
}
