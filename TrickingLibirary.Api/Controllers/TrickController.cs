using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.Form;
using TrickingLibirary.Api.Helpers;
using TrickingLibirary.Api.ViewModels;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Entities.Modertion;
using TrickingLibirary.Domain.Interfaces;
using TrickingLibirary.Infrastructure.Data.Helpers;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrickController : ApiController
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

    #region Endpoints :
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Tricks.AsNoTracking()
            .Where(x => x.Active)
            .Include(x => x.TrickCategories)
            .Include(x => x.Prerequisites)
            .Include(x => x.Progressions)
            .Include(x=>x.User)
            .Select(TrickViewModels.UserProjection).ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var query = dbContext.Tricks.AsQueryable();
        query = int.TryParse(id, out int intId) ? query.Where(x => x.Id.Equals(intId)) :
             query.Where(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase) && x.Active);

        var trick = query
            .Include(x => x.TrickCategories)
            .Include(x => x.Prerequisites)
            .Include(x => x.Progressions)
            .Include(x => x.User)
            .Select(TrickViewModels.FullProjection).FirstOrDefault();

        return trick is null ? NoContent() : Ok(trick);
    }
    [HttpGet("{trickId}/submissions")]
    public IActionResult ListSubmissionsForTrick(string trickId)
    {
        return Ok(dbContext.Submissions.Include(x => x.Video).Include(x=>x.User)
            .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
            .Select(SubmissionViewModels.Projection).ToList());
    }
    [HttpPost]
    [Authorize(Policy = Tricking_LibiraryConstants.Policies.User)]
    public async Task<IActionResult> Create([FromBody] TrickForm trickForm)
    {
        Trick trick = new()
        {
            Slug = trickForm.Name.Replace(".", "-").ToLowerInvariant(),
            Name = trickForm.Name,
            Version = 1,
            Description = trickForm.Description,
            Difficulty = trickForm.Difficulty,
            Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x }).ToList(),
            Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
            TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
            UserId=UserId
        };
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        dbContext.ModerationItems.Add(new ModerationItem
        {
            Target = trick.Id,
            Type = MpderationTypes.Trick
        });
        await dbContext.SaveChangesAsync();
        return Ok(TrickViewModels.Create(trick));
    }
    [HttpPut]
    [Authorize(Policy =Tricking_LibiraryConstants.Policies.User)]
    public async Task<IActionResult> Update([FromBody] TrickForm trickForm)
    {
        var trick = await dbContext.Tricks.FirstOrDefaultAsync(x => x.Id.Equals(trickForm.Id));
        if (trick is null) return NoContent();

        var newTrick = new Trick
        {
            Slug = trick.Slug,
            Name = trick.Name,
            Version = trick.Version + 1,
            Description = trickForm.Description,
            Difficulty = trickForm.Difficulty,
            Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x }).ToList(),
            Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
            TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
            UserId = UserId
        };
        dbContext.Tricks.Add(newTrick);
        await dbContext.SaveChangesAsync();
        dbContext.ModerationItems.Add(new ModerationItem
        {
            Current = trick.Id,
            Target = newTrick.Id,
            Type = MpderationTypes.Trick
        });
        await dbContext.SaveChangesAsync();
        return Ok(TrickViewModels.Create(newTrick));
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var trick = dbContext.Tricks.FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        trick.Deleted = true;
        await dbContext.SaveChangesAsync();
        return Ok();
    }
    #endregion
}
