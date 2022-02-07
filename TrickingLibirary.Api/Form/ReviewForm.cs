using TrickingLibirary.Domain.Entities.Modertion;

namespace TrickingLibirary.Api.Form;

public class ReviewForm
{
    public string Comment { get; set; }
    public ReviewStatus ReviewStatus { get; set; }
}
