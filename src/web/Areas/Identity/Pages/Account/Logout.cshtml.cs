using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace tomware.Microsts.Web.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LogoutModel : PageModel
  {
    public string LogoutId { get; set; }

    private readonly ILogger<LogoutModel> _logger;
    private readonly IEventService _eventService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(
      ILogger<LogoutModel> logger,
      IEventService eventService,
      SignInManager<ApplicationUser> signInManager
    )
    {
      _logger = logger;
      _signInManager = signInManager;
      _eventService = eventService;
    }

    public void OnGet(string logoutId)
    {
      this.LogoutId = logoutId;
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
      await _signInManager.SignOutAsync();

      await _eventService.RaiseAsync(new UserLogoutSuccessEvent(
        User.GetSubjectId(),
        User.GetDisplayName()
      ));

      _logger.LogInformation("User logged out.");

      if (returnUrl != null)
      {
        return LocalRedirect(returnUrl);
      }
      else
      {
        return RedirectToPage();
      }
    }
  }
}
