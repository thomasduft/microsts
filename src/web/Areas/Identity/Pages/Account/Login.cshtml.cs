using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using tomware.Microsts.Web.Resources;

namespace tomware.Microsts.Web.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class LoginModel : PageModel
  {
    private readonly ILogger<LoginModel> logger;
    private readonly IEventService eventService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IIdentityServerInteractionService interaction;
    private readonly IdentityLocalizationService identityLocalizationService;

    public LoginModel(
      ILogger<LoginModel> logger,
      IEventService eventService,
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IIdentityServerInteractionService interaction,
      IdentityLocalizationService identityLocalizationService
    )
    {
      this.logger = logger;
      this.eventService = eventService;
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.interaction = interaction;
      this.identityLocalizationService = identityLocalizationService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
      [Required(ErrorMessage = "USERNAME_REQUIRED")]
      [Display(Name = "Username")]
      // [EmailAddress]
      public string Username { get; set; }

      [Required(ErrorMessage = "PASSWORD_REQUIRED")]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
      if (!string.IsNullOrEmpty(ErrorMessage))
      {
        ModelState.AddModelError(string.Empty, ErrorMessage);
      }

      returnUrl = returnUrl ?? Url.Content("~/");

      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");

      var context = await interaction.GetAuthorizationContextAsync(returnUrl);

      if (ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await signInManager.PasswordSignInAsync(
          Input.Username,
          Input.Password,
          Input.RememberMe,
          lockoutOnFailure: true
        );
        if (result.Succeeded)
        {
          logger.LogInformation("User logged in.");

          var user = await userManager.FindByNameAsync(Input.Username);
          await eventService.RaiseAsync(new UserLoginSuccessEvent(
            user.UserName,
            user.Id,
            user.UserName,
            clientId: context?.ClientId
          ));

          return LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
          return RedirectToPage("./LoginWith2fa", new
          {
            ReturnUrl = returnUrl,
            RememberMe = Input.RememberMe
          });
        }
        if (result.IsLockedOut)
        {
          logger.LogWarning("User account locked out.");
          return RedirectToPage("./Lockout");
        }
        else
        {
          ModelState.AddModelError(
            string.Empty,
            identityLocalizationService.GetLocalizedHtmlString("INVALID_LOGIN_ATTEMPT")
          );

          return Page();
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
