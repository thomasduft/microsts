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
    private readonly ILogger _logger;
    private readonly IAccountService _accountService;

    public AccountController(
      ILoggerFactory loggerFactory,
      IAccountService accountService)
    {
      _logger = loggerFactory.CreateLogger<AccountController>();
      _accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(IdentityResult), 200)]
    public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser user = _accountService.CreateUser(model);
        var result = await _accountService.RegisterAsync(user, model.Password);
        if (result.Succeeded)
        {
          _logger.LogInformation(3, $"New user {user.Email} successfully registred!");

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
        var result = await _accountService.ChangePasswordAsync(model);
        if (result.Succeeded)
        {
          _logger.LogInformation(3, $"The password for user {model.UserName} has been changed.");

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
        var result = await _accountService.AddRoleAsync(model.RoleName);
        if (result.Succeeded)
        {
          _logger.LogInformation(3, $"The role {model.RoleName} has been added.");

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
        var result = await _accountService.AssignRoleAsync(model);
        if (result.Succeeded)
        {
          _logger.LogInformation(3, $"The role {model.RoleName} has been assigned.");

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
        var result = await _accountService.UnassignRoleAsync(model);
        if (result.Succeeded)
        {
          _logger.LogInformation(3, $"The role {model.RoleName} has been unassigned.");

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
      return Ok(_accountService.GetRoles());
    }

    [HttpGet]
    [Route("users")]
    [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
    public IActionResult Users()
    {
      return Ok(_accountService.GetUsers());
    }

    [HttpGet]
    [Route("user/{id}")]
    [ProducesResponseType(typeof(UserViewModel), 200)]
    public async Task<IActionResult> GetUser(string id)
    {
      var result = await this._accountService.GetUser(id);
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
