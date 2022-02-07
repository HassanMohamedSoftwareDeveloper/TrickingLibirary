using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Video:TemporalModel
{
    public int Id { get; set; }
    public string VideoLink { get; set; }
    public string ThumbLink { get; set; }
}
