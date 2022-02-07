using System.Linq.Expressions;
using TrickingLibirary.Domain.Entities.Modertion;

namespace TrickingLibirary.Api.ViewModels;

public class ReviewViewModel
{
    public static readonly Func<Review, object> Create = Projection.Compile();
    public static Expression<Func<Review, object>> Projection =>
    review => new
    {
        review.Id,
        review.ModerationItemId,
        review.Comment,
        review.ReviewStatus
    };
}
