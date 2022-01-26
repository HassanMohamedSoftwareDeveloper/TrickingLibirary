namespace TrickingLibirary.Domain.Entities;

public abstract class BaseModel<TId> :BaseEntity<TId>  
{
    public bool Deleted { get; set; }
}
