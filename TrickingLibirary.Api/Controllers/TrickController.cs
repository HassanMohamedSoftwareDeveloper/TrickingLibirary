using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrickController : ControllerBase
{
    #region Fields :
    public static readonly List<Trick> Tricks = new List<Trick>();
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public TrickController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Tricks.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(dbContext.Tricks.FirstOrDefault(x => x.Id.Equals(id,StringComparison.InvariantCultureIgnoreCase)));
    }
    [HttpGet("{trickId}/submissions")]
    public IActionResult ListSubmissionsForTrick(string trickId)
    {
        return Ok(dbContext.Submissions.Where(x => x.TrickId.Equals(trickId,StringComparison.InvariantCultureIgnoreCase)).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Trick trick)
    {
        trick.Id = trick.Name.Replace(".", "-").ToLowerInvariant();
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(trick);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Trick trick)
    {
        if (string.IsNullOrWhiteSpace(trick.Id)) return null;
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(trick);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var trick = dbContext.Tricks.FirstOrDefault(x => x.Id.Equals(id,StringComparison.InvariantCultureIgnoreCase));
        trick.Deleted = true;
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
