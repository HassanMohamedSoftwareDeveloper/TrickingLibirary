using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;
using TrickingLibirary.Api.Form;
using TrickingLibirary.Domain.Entities;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SubmissionsController : ControllerBase
{
    #region Fields :
    public static readonly List<Trick> Tricks = new List<Trick>();
    private readonly IDbContext dbContext;
    #endregion

    #region CTORS :
    public SubmissionsController(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    #endregion
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(dbContext.Submissions.Where(x=>x.VideoProcessed).ToList());
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(dbContext.Submissions.FirstOrDefault(x => x.Id.Equals(id)));
    }
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] Channel<EditVideoMessage> channel, 
        [FromServices]VideoManager videoManager,
        [FromBody] SubmissionForm submissionForm)
    {
        if (videoManager.CheckTemporaryFileIsExist(submissionForm.Video) is false) return BadRequest();
        Submission submission = new()
        {
            TrickId=submissionForm.TrickId,
            Description=submissionForm.Description,
            VideoProcessed = false,
        };
        dbContext.Submissions.Add(submission);
        await dbContext.SaveChangesAsync();
        await channel.Writer.WriteAsync(new EditVideoMessage
        {
            SubmissionId = submission.Id,
            Input = submissionForm.Video,
        });
        return Ok(submission);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Submission submission)
    {
        if (submission.Id == 0) return null;
        dbContext.Submissions.Add(submission);
        await dbContext.SaveChangesAsync();
        return Ok(submission);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var submission = dbContext.Submissions.FirstOrDefault(x => x.Id.Equals(id));
        if (submission is null) return NotFound();
        submission.Deleted = true;
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
