namespace TrickingLibirary.Domain.Entities;

public class Category:BaseModel<string>
{
    public string Description { get; set; }
    public IList<TrickCategory> Tricks { get; set; }
}
