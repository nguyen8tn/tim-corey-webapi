using ApiSecurity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TimCoreyWebApi.Controllers.v1;

//[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Route("api/[controller]")]
//[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;

    public UsersController(IConfiguration config)
    {
        _config = config;
    }

    // GET: api/<UsersController>
    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<UsersController>/5
    [HttpGet("{id}")]
    [Authorize(Policy = PolicyConstants.MustHaveEmployeeId)]
    [Authorize(Policy = PolicyConstants.MustBeAVeteranEmployee)]
    public string Get(int id)
    {
        return _config.GetConnectionString("Default");
    }

    // POST api/Users
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
