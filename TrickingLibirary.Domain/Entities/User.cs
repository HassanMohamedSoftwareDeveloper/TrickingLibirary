using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class User : BaseModel<string>
{
    public string Username { get; set; }
    public string Image { get; set; }
    public IList<Submission> Submissions { get; set; } = new List<Submission>();
}
