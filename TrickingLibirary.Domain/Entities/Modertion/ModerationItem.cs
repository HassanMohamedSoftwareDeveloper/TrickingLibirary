namespace TrickingLibirary.Domain.Entities.Modertion;

public class ModerationItem:BaseModel<int>
{
    public string Target { get; set; }
    public string Type { get; set; }
}
