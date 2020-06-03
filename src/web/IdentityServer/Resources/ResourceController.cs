using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tomware.Microsts.Web
{
  [Route("api/resources")]
  [SecurityHeaders]
  [Authorize(Policies.ADMIN_POLICY)]
  public class ResourceController : Controller
  {
    private readonly IResourceService service;

    public ResourceController(IResourceService service)
    {
      this.service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ApiResourceViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApiResourcesAsync()
    {
      var result = await this.service.GetApiResourcesAsync();

      return Ok(result);
    }

    [HttpGet("names")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResourceNamesAsync()
    {
      var result = await this.service.GetResourceNamesAsync();

      return Ok(result);
    }

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(ApiResourceViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(string name)
    {
      if (name == null) return BadRequest();

      var result = await this.service.GetApiResourceAsync(name);
      if (result == null) return NotFound();

      return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] ApiResourceViewModel model)
    {
      if (model == null) return BadRequest();
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var result = await this.service.CreateApiResourceAsync(model);

      return Created($"api/resources/{result}", this.Json(result));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync([FromBody] ApiResourceViewModel model)
    {
      if (model == null) return BadRequest();
      if (!ModelState.IsValid) return BadRequest(ModelState);

      await this.service.UpdateApiResourceAsync(model);

      return NoContent();
    }

    [HttpDelete("{name}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteApiResourceAsync(string name)
    {
      if (name == null) return BadRequest();

      // TODO: check if still used anywhere?
      await this.service.DeleteApiResourceAsync(name);

      return NoContent();
    }
  }
}