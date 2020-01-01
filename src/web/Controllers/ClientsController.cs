using Microsoft.AspNetCore.Mvc;

namespace tomware.STS.Web
{
  [Route("api/clients")]
  public class ClientsController : Controller
  {
    [HttpGet]
    [Route("config/{name}")]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult Config(string name)
    {
      //TODO: return config for client!
      return Ok(name);
    }
  }
}
