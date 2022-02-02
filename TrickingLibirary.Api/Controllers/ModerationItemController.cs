using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Api.ViewModels;
using TrickingLibirary.Domain.Entities;
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
        var moderationItem = dbContext.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));
        if (moderationItem is null) return NoContent();
        var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");//regex 1:1

        comment.HtmlContent = regex.Matches(comment.Content)
            .Aggregate(comment.Content, (content, match) =>
            {
                var tag = match.Groups["tag"].Value;
                return content.Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
            });

        moderationItem.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
        return Ok(CommentViewModel.Create(comment));
    }
    #endregion
}
