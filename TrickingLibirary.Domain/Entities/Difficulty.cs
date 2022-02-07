using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Difficulty : SlugModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Trick> Tricks { get; set; }
}
