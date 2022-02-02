using System.Linq.Expressions;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Api.ViewModels;

public static class CommentViewModel
{
    public static readonly Func<Comment, object> Create = Projection.Compile();
    public static Expression<Func<Comment, object>> Projection =>
    comment => new
    {
        comment.Id,
        comment.ParentId,
        comment.Content,
        comment.HtmlContent,
    };
}
