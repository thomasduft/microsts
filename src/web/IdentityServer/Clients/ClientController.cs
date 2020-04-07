using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tomware.Microsts.Web
{
  [Route("api/clients")]
  [SecurityHeaders]
  public class ClientController : Controller
  {
    private readonly IClientService service;
    private readonly IClientConfigurationService configurationService;

    public ClientController(
      IClientService service,
      IClientConfigurationService configurationService
    )
    {
      this.service = service;
      this.configurationService = configurationService;
    }

    [HttpGet]
    // [Authorize(Policies.ADMIN_POLICY)]
    [ProducesResponseType(typeof(IEnumerable<ClientViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientsAsync()
    {
      var result = await this.service.GetClientsAsync();

      return Ok(result);
    }

    [HttpGet("{clientId}")]
    // [Authorize(Policies.ADMIN_POLICY)]
    [ProducesResponseType(typeof(ClientViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(string clientId)
    {
      if (clientId == null) return BadRequest();

      var result = await this.service.GetAsync(clientId);
      if (result == null) return NotFound();

      return Ok(result);
    }

    [HttpPost]
    // [Authorize(Policies.ADMIN_POLICY)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody]ClientViewModel model)
    {
      if (model == null) return BadRequest();
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var result = await this.service.CreateAsync(model);

      return Created($"api/clients/{result}", result);
    }

    [HttpPut]
    // [Authorize(Policies.ADMIN_POLICY)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync([FromBody]ClientViewModel model)
    {
      if (model == null) return BadRequest();
      if (!ModelState.IsValid) return BadRequest(ModelState);

      await this.service.UpdateAsync(model);

      return NoContent();
    }

    [HttpDelete("{clientId}")]
    // [Authorize(Policies.ADMIN_POLICY)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(string clientId)
    {
      if (clientId == null) return BadRequest();

      await this.service.DeleteAsync(clientId);

      return NoContent();
    }

    // #############################################################################################
    // TODO: will be deprecated...or refactore to sth. like hardcode FrontendConfig endpoint

    [HttpGet("config/{clientId}")]
    [ProducesResponseType(typeof(ClientConfigurationViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Config(string clientId)
    {
      if (clientId is null) return BadRequest();

      var result = await this.configurationService.GetClient(clientId);
      if (result == null) return NotFound();

      return Ok(result);
    }
  }
}
