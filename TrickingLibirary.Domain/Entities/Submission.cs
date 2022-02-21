using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Submission : TemporalModel
{
    public int VideoId { get; set; }
    public Video Video { get; set; }
    public string TrickId { get; set; }
    public string Description { get; set; }
    public bool VideoProcessed { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
