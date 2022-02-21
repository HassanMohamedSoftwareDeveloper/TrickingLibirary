namespace TrickingLibirary.Domain.Entities.Abstractions;

public abstract class TemporalModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
}
