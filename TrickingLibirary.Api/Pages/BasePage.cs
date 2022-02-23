using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrickingLibirary.Api.Pages;

public class BasePage : PageModel
{
    public IList<string> CustomErrors { get; set; } = new List<string>();
}
