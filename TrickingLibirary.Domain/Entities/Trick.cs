using TrickingLibirary.Domain.Entities.Abstractions;

namespace TrickingLibirary.Domain.Entities;

public class Trick : VersionModel
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Difficulty { get; set; }

    public IList<TrickRelationship> Prerequisites { get; set; } = new List<TrickRelationship>();
    public IList<TrickRelationship> Progressions { get; set; } = new List<TrickRelationship>();
    public IList<TrickCategory> TrickCategories { get; set; } = new List<TrickCategory>();
}
