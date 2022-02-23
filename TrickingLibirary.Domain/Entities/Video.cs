using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Video: BaseModel<int>
{
    public string VideoLink { get; set; }
    public string ThumbLink { get; set; }
}
