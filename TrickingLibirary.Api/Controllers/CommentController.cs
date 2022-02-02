using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Api.ViewModels;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public CommentController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion

    #region Endpoints :
    [HttpGet("{id}/replies")]
    public IEnumerable<object> GetReplies(int id)
        => dbContext.Comments.Where(x => x.ParentId.Equals(id)).Select(CommentViewModel.Projection).ToList();
    [HttpPost("{id}/replies")]
    public async Task<IActionResult> Reply(int id, [FromBody] Comment reply)
    {
        var comment = dbContext.Comments.FirstOrDefault(x => x.Id.Equals(id));
        if (comment is null) return NoContent();
        var regex = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)");//regex 1:1

        reply.HtmlContent = regex.Matches(reply.Content)
            .Aggregate(reply.Content, (content, match) =>
            {
                var tag = match.Groups["tag"].Value;
                return content.Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
            });

        comment.Replies.Add(reply);
        await dbContext.SaveChangesAsync();
        return Ok(CommentViewModel.Create(reply));
    }
    #endregion
}
