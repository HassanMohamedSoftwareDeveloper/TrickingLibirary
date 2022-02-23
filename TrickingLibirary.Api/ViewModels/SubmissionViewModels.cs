using System.Linq.Expressions;
using TrickingLibirary.Domain.Entities;

namespace TrickingLibirary.Api.ViewModels;

public static class SubmissionViewModels
{
    public static readonly Func<Submission, object> Create = Projection.Compile();
    public static Expression<Func<Submission, object>> Projection =>
        submission => new
        {
            submission.Id,
            submission.Description,
            Thumb = submission.Video.ThumbLink,
            Video = submission.Video.VideoLink,
            User = new
            {
                submission.User.Username,
                submission.User.Image,
            },
        };
}
