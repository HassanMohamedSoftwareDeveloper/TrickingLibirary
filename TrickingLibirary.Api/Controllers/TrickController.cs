using Microsoft.AspNetCore.Mvc;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrickController : ControllerBase
{
    public static List<Trick> Tricks = new List<Trick>();
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Tricks);
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(Tricks.FirstOrDefault(x=>x.Id==id));
    }
    [HttpPost]
    public IActionResult Create([FromBody]Trick trick)
    {
         Tricks.Add(trick);
        return Ok();
    }
}

public class Trick
{
    public int Id { get; set; }
    public string TrickName { get; set; }
    public string Video { get; set; }
}
