namespace TrickingLibirary.Domain.Entities;

public class Submission : BaseModel<int>
{
    public string Video { get; set; }
    public string TrickId { get; set; }
    public string Description { get; set; }
    public bool VideoProcessed { get; set; }
}
