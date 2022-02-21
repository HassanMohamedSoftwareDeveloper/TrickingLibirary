using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    #region Fields :
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public CategoriesController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Categories.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(dbContext.Categories.FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase)));
    }
    [HttpGet("{id}/tricks")]
    public IActionResult ListCategoryTricks(int id)
    {
        return Ok(dbContext.TrickCategories
            .Include(x => x.Trick)
            .Where(x => x.CategoryId.Equals(id))
            .Select(x => x.Trick).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        category.Slug = category.Name.Replace(".", "-").ToLowerInvariant();
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
        return Ok(category);
    }
}
