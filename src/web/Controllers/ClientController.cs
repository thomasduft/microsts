using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tomware.STS.Web
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

    [HttpGet]
    [Route("config/{clientId}")]
    [ProducesResponseType(typeof(ClientConfigurtationViewModel), 200)]
    public async Task<IActionResult> Config(string clientId)
    {
      if (clientId is null) return BadRequest();

      var result = await this.service.GetClient(clientId);
      if (result == null) return NotFound();

      return Ok(result);
    }
  }
}
