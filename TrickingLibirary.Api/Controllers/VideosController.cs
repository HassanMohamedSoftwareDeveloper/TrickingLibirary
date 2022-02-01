using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    #region Fields :
    private readonly VideoManager videoManager;
    #endregion

    #region CTORS :
    public VideosController(VideoManager videoManager)
    {
        this.videoManager = videoManager;
    }
    #endregion

    #region Endpoints :
    [HttpGet("{video}")]
    public IActionResult GetVideo(string video)
    {
        var savePath = videoManager.GenerateDevVideoPath(video);
        if (string.IsNullOrWhiteSpace(savePath)) return BadRequest();
        return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite), MediaTypeHeaderValue.Parse("video/*"));
    }
    [HttpPost]
    public Task<string> UploadVideo(IFormFile video)
    {
        return videoManager.SaveTemporaryVideo(video);      
    }
    [HttpDelete("{fileName}")]
    public IActionResult DeleteTemporaryVideo(string fileName)
    {
        if (videoManager.CheckIsTemporary(fileName) is false) return BadRequest();
        if (videoManager.CheckTemporaryFileIsExist(fileName) is false) return NoContent();
        videoManager.DeleteTemporaryFile(fileName);
        return Ok();
    } 
    #endregion
}
