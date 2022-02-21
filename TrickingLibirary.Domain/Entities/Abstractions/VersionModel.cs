namespace TrickingLibirary.Domain.Entities.Abstractions;

public abstract class VersionModel : TemporalModel
{
   
    public int Version { get; set; }
    public bool Active { get; set; }
    public DateTime Timestamp { get; set; }
}
