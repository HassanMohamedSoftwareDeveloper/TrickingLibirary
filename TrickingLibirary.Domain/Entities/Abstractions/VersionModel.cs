namespace TrickingLibirary.Domain.Entities.Abstractions;

public abstract class VersionModel : BaseModel<int>
{
    public int Version { get; set; }
    public bool Active { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
