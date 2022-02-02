using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.Form;
using TrickingLibirary.Api.ViewModels;
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
        return Ok(dbContext.Tricks.Select(TrickViewModels.Projection).ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(dbContext.Tricks
            .Where(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .Select(TrickViewModels.Projection).FirstOrDefault());
    }
    [HttpGet("{trickId}/submissions")]
    public IActionResult ListSubmissionsForTrick(string trickId)
    {
        return Ok(dbContext.Submissions.Include(x=>x.Video)
            .Where(x => x.TrickId.Equals(trickId,StringComparison.InvariantCultureIgnoreCase)).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrickForm trickForm)
    {
        Trick trick = new()
        {
            Id = trickForm.Name.Replace(".", "-").ToLowerInvariant(),
            Name = trickForm.Name,
            Description = trickForm.Description,
            Difficulty = trickForm.Difficulty,
            TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList()
        };
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(TrickViewModels.Create(trick));
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Trick trick)
    {
        if (string.IsNullOrWhiteSpace(trick.Id)) return null;
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(TrickViewModels.Create(trick));
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
