using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DifficultiesController : ControllerBase
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public DifficultiesController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Difficulties.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(dbContext.Difficulties.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)));
    }
    [HttpGet("{id}/tricks")]
    public IActionResult ListDifficultyTricks(string id)
    {
        return Ok(dbContext.Tricks
            .Where(x => x.Difficulty.Equals(id, StringComparison.InvariantCultureIgnoreCase)).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Difficulty difficulty)
    {
        difficulty.Id = difficulty.Name.Replace(".", "-").ToLowerInvariant();
        dbContext.Difficulties.Add(difficulty);
        await dbContext.SaveChangesAsync();
        return Ok(difficulty);
    }
}
