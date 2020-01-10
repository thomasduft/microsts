using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tomware.Microsts.Web
{
  [Route("api/account")]
  [SecurityHeaders]
  [Authorize(Policies.ADMIN_POLICY)]
  public class AccountController : Controller
  {
    private readonly ILogger<AccountController> logger;
    private readonly IAccountService accountService;

    public AccountController(
      ILogger<AccountController> logger,
      IAccountService accountService
    )
    {
      this.logger = logger;
      this.accountService = accountService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody]RegisterUserViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser user = this.accountService.CreateUser(model);
        var result = await this.accountService.RegisterAsync(user, model.Password);
        if (result.Succeeded)
        {
          this.logger.LogInformation(
            "New user {Email} successfully registred!",
            user.Email
          );

          return Ok(result);
        }

        AddErrors(result);
      }

      return BadRequest(ModelState);
    }

    [HttpPost("changepassword")]
    [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.ChangePasswordAsync(model);
        if (result.Succeeded)
        {
          this.logger.LogInformation(
            "The password for user {UserName} has been changed.",
            model.UserName
          );

          return Ok(result);
        }

        AddErrors(result);
      }

      return BadRequest(ModelState);
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(IEnumerable<UserViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Users()
    {
      var result = await this.accountService.GetUsersAsync();

      return Ok(result);
    }

    [HttpGet("user/{id}")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(string id)
    {
      var result = await this.accountService.GetUserAsync(id);

      return Ok(result);
    }

    [HttpPut("assignclaims")]
    [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignClaimsAsync([FromBody]AssignClaimsViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.AssignClaimsAsync(model);
        if (result.Succeeded)
        {
          this.logger.LogInformation("The claims have been assigned.");

          return Ok(result);
        }

        AddErrors(result);
      }

      return BadRequest(ModelState);
    }

    [HttpPut("assignroles")]
    [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRolesAsync([FromBody]AssignRolesViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.AssignRolesAsync(model);
        if (result.Succeeded)
        {
          this.logger.LogInformation("The roles have been assigned.");

          return Ok(result);
        }

        AddErrors(result);
      }

      return BadRequest(ModelState);
    }

    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }
  }
}
