namespace TrickingLibirary.Domain.Entities.Abstractions;

public abstract class VersionModel : TemporalModel
{
    public int Version { get; set; }
    public bool Temporary { get; set; } = true;
    public bool Active { get; set; }
    public DateTime Timestamp { get; set; }
}
