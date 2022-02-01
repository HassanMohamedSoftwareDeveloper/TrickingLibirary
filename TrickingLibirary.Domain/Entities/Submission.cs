namespace TrickingLibirary.Domain.Entities;

public class Submission : BaseModel<int>
{
    public int VideoId { get; set; }
    public Video Video { get; set; }
    public string TrickId { get; set; }
    public string Description { get; set; }
    public bool VideoProcessed { get; set; }
}
