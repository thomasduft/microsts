using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tomware.STS.Web
{
  [Route("api/account")]
  [Authorize(Policies.ADMIN_POLICY)]
  public class AccountController : Controller
  {
    private readonly ILogger logger;
    private readonly IAccountService accountService;

    public AccountController(
      ILoggerFactory loggerFactory,
      IAccountService accountService)
    {
      this.logger = loggerFactory.CreateLogger<AccountController>();
      this.accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
    public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
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

    [HttpPost]
    [Authorize]
    [Route("changepassword")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
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

        return Ok(result);
      }

      return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("addrole")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
    public async Task<IActionResult> AddRole([FromBody]AddRoleViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.AddRoleAsync(model.RoleName);
        if (result.Succeeded)
        {
          this.logger.LogInformation(
            "The role {Role} has been added.",
            model.RoleName
          );

          return Ok(result);
        }
      }

      return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("assignrole")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
    public async Task<IActionResult> AssignRole([FromBody]AssignViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.AssignRoleAsync(model);
        if (result.Succeeded)
        {
          this.logger.LogInformation(
            "The role {Role} has been assigned.",
            model.RoleName
          );

          return Ok(result);
        }
      }

      return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("unassignrole")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
    public async Task<IActionResult> UnassignRole([FromBody]AssignViewModel model)
    {
      if (ModelState.IsValid)
      {
        var result = await this.accountService.UnassignRoleAsync(model);
        if (result.Succeeded)
        {
          this.logger.LogInformation(
            "The role {Role} has been unassigned.",
            model.RoleName
          );

          return Ok(result);
        }
      }

      return BadRequest(ModelState);
    }

    [HttpGet]
    [Route("roles")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public IActionResult Roles()
    {
      return Ok(this.accountService.GetRoles());
    }

    [HttpGet]
    [Route("users")]
    [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
    public IActionResult Users()
    {
      return Ok(this.accountService.GetUsers());
    }

    [HttpGet]
    [Route("user/{id}")]
    [ProducesResponseType(typeof(UserViewModel), 200)]
    public async Task<IActionResult> GetUser(string id)
    {
      var result = await this.accountService.GetUser(id);
      return Ok(result);
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
