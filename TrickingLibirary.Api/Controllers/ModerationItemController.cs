using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.ViewModels;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Entities.Modertion;
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
    public async Task<IActionResult> Review(int id, [FromBody] Review review)
    {
        if (dbContext.ModerationItems.Any(x => x.Id.Equals(id)) is false) return NoContent();
        review.ModerationItemId = id;
        dbContext.Reviews.Add(review);
        await dbContext.SaveChangesAsync();
        return Ok(review);
    }
    #endregion
}
