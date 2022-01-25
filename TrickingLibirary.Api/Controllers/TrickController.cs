﻿using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrickController : ControllerBase
{
    #region Fields :
    public static readonly List<Trick> Tricks = new List<Trick>();
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public TrickController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Tricks.ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(dbContext.Tricks.FirstOrDefault(x => x.Id.Equals(id)));
    }
    [HttpGet("{trickId}/submissions")]
    public IActionResult ListSubmissionsForTrick(int trickId)
    {
        return Ok(dbContext.Submissions.Where(x => x.TrickId.Equals(trickId)).ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Trick trick)
    {
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(trick);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Trick trick)
    {
        if (trick.Id == 0) return null;
        dbContext.Tricks.Add(trick);
        await dbContext.SaveChangesAsync();
        return Ok(trick);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var trick = dbContext.Tricks.FirstOrDefault(x => x.Id.Equals(id));
        trick.Deleted = true;
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
