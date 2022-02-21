using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.Form;
using TrickingLibirary.Api.ViewModels;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Entities.Modertion;
using TrickingLibirary.Domain.Interfaces;
using TrickingLibirary.Infrastructure.Data;

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
        return Ok(dbContext.ModerationItems.Where(x => x.Deleted.Equals(false)).ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(dbContext.ModerationItems
            .Include(x => x.Comments)
            .Include(x => x.Reviews)
            .Where(x => x.Id.Equals(id))
            .Select(ModerationItemViewModels.Projection)
            .FirstOrDefault());
    }

    [HttpGet("{id}/comments")]
    public IActionResult GetComments(int id)
    {
        return Ok(dbContext.Comments
            .Where(x => x.ModerationItemId.Equals(id))
            .Select(CommentViewModel.Projection)
            .ToList());
    }
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> Comment(int id, [FromBody] Comment comment)
    {
        if (dbContext.ModerationItems.Any(x => x.Id.Equals(id)) is false) return NoContent();
        var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");//regex 1:1

        comment.HtmlContent = regex.Matches(comment.Content)
            .Aggregate(comment.Content, (content, match) =>
            {
                var tag = match.Groups["tag"].Value;
                return content.Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
            });

        comment.ModerationItemId = id;
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
        return Ok(CommentViewModel.Create(comment));
    }

    [HttpGet("{id}/reviews")]
    public IActionResult GetReviews(int id)
    {
        return Ok(dbContext.Reviews
            .Where(x => x.ModerationItemId.Equals(id))
            .ToList());
    }
    [HttpPost("{id}/reviews")]
    public async Task<IActionResult> Review(int id,
        [FromBody] ReviewForm reviewForm,
        [FromServices] VersionMigrationContext migrationContext
        )
    {
        var modItem = dbContext.ModerationItems
            .Include(x => x.Reviews).FirstOrDefault(x => x.Id.Equals(id));
        if (modItem is null) return NoContent();

        if (modItem.Deleted) return BadRequest("Moderation item no longer exist.");

        var review = new Review
        {
            ModerationItemId = id,
            Comment = reviewForm.Comment,
            ReviewStatus = reviewForm.ReviewStatus,
        };

        try
        {
            if (modItem.Reviews.Count >= 3)
            {
                migrationContext.Migrate(modItem);
                modItem.Deleted = true;
            }
        }
        catch (InvalidVersionException ex)
        {

            return BadRequest(ex.Message);
        }

        dbContext.Reviews.Add(review);


        await dbContext.SaveChangesAsync();
        return Ok(ReviewViewModel.Create(review));
    }
    #endregion
}
