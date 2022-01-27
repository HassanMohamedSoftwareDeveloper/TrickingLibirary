namespace TrickingLibirary.Domain.Entities;

public class TrickRelationship
{
    public string PrerequisiteId { get; set; }
    public Trick Prerequisite { get; set; }
    public string ProgressionId { get; set; }
    public Trick Progression { get; set; }
}
