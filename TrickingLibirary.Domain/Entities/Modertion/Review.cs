using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities.Modertion;

public class Review : TemporalModel
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public int ModerationItemId { get; set; }
    public ModerationItem ModerationItem { get; set; }
    public ReviewStatus ReviewStatus { get; set; }

}
public enum ReviewStatus
{
    Approved = 0,
    Rejected,
    Waiting
}
