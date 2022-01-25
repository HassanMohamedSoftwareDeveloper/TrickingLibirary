namespace TrickingLibirary.Domain.Entities;

public abstract class BaseModel<TId> :BaseEntity<TId> where TId : struct
{
    public bool Deleted { get; set; }
}
