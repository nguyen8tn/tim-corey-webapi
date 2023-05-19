using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VersionedApi.Controllers;

//[Route("api/v{version:apiVersion}/[controller]")]
[Route("[controller]")]
[ApiController]
//[ApiVersionNeutral]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("ping")]
    [AllowAnonymous]
    public IActionResult Ping()
    {
        return Ok("Everything seems great!");
    }
}
