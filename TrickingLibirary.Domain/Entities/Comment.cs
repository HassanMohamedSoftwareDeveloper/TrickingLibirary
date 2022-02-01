namespace TrickingLibirary.Domain.Entities;

public class Comment:BaseModel<int>
{
    public string Content { get; set; }
    public string HtmlContent { get; set; }

}
