namespace TrickingLibirary.Domain.Entities;

public class Video:BaseEntity<int>
{
    public string VideoLink { get; set; }
    public string ThumbLink { get; set; }
}
