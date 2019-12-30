using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tomware.STS.Web.Pages
{
  public class LoginModel : PageModel
  {
    private readonly ILogger<LoginModel> logger;
    private readonly SignInManager<ApplicationUser> signInManager;

    [Required]
    [BindProperty]
    public string Username { get; set; }

    [Required]
    [BindProperty]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [BindProperty]
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    [BindProperty]
    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public LoginModel(
      ILogger<LoginModel> logger,
      SignInManager<ApplicationUser> signInManager
    )
    {
      this.logger = logger;
      this.signInManager = signInManager;
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
      if (!string.IsNullOrEmpty(this.ErrorMessage))
      {
        this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
      }

      returnUrl = returnUrl ?? this.Url.Content("~/");

      // Clear the existing external cookie to ensure a clean login process
      await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      this.ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
      returnUrl = returnUrl ?? this.Url.Content("~/");

      if (this.ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await this.signInManager.PasswordSignInAsync(this.Username, this.Password, this.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          this.logger.LogInformation("User logged in.");
          return this.LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
          return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = this.RememberMe });
        }
        if (result.IsLockedOut)
        {
          this.logger.LogWarning("User account locked out.");
          return this.RedirectToPage("./Lockout");
        }
        else
        {
          this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return this.Page();
        }
      }

      return this.Page();
    }
  }
}