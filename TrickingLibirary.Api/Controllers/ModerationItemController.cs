using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ModerationItemController : ControllerBase
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public ModerationItemController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Endpoints :
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.ModerationItems.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(dbContext.ModerationItems.FirstOrDefault(x => x.Id.Equals(id)));
    }
    #endregion
}
