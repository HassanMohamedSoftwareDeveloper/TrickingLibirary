namespace TrickingLibirary.Domain.Entities;

public class Difficulty : BaseModel<string>
{
    public string Description { get; set; }
    public IList<Trick> Tricks { get; set; }
}
