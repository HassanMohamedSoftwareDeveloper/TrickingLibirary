using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Category:SlugModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<TrickCategory> Tricks { get; set; }
}
