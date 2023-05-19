using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VersionedApi.Controllers.v1
{
    //[Route("api/v{verion:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiVersion("1.0", Deprecated = true)]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
