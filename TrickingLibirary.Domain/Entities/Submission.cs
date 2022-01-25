namespace TrickingLibirary.Domain.Entities;

public class Submission : BaseModel<int>
{
    public string Video { get; set; }
    public int TrickId { get; set; }
    public string Description { get; set; }
}
