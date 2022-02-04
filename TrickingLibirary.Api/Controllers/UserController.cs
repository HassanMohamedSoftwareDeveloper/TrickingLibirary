using Microsoft.AspNetCore.Mvc;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region Endpoints :
    [HttpGet("me")]
    public IActionResult GetMe() => Ok();
    [HttpGet("{id}")]
    public IActionResult GetUser(string id) => Ok(); 
    #endregion
}
