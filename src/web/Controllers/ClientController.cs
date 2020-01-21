using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tomware.Microsts.Web
{
  [Route("api/client")]
  [SecurityHeaders]
  public class ClientController : Controller
  {
    private readonly IClientConfigurationService service;

    public ClientController(IClientConfigurationService service)
    {
      this.service = service;
    }

    [HttpGet("config/{clientId}")]
    [ProducesResponseType(typeof(ClientConfigurationViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Config(string clientId)
    {
      if (clientId is null) return BadRequest();

      var result = await this.service.GetClient(clientId);
      if (result == null) return NotFound();

      return Ok(result);
    }
  }
}
