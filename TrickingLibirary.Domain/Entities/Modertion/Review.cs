namespace TrickingLibirary.Domain.Entities.Modertion;

public class Review : BaseModel<int>
{
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
