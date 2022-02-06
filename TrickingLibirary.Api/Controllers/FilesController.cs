using Microsoft.AspNetCore.Mvc;
using TrickingLibirary.Api.BackgroundServices.VideoEditing;
using TrickingLibirary.Api.Helpers;

namespace TrickingLibirary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    #region Fields :
    private readonly IFileManager fileManager;
    #endregion

    #region CTORS :
    public FilesController(IFileManager fileManager)
    {
        this.fileManager = fileManager;
    }
    #endregion

    #region Endpoints :
    [HttpGet("{type}/{file}")]
    public IActionResult GetFile(string type, string file)
    {
        bool isParsed = Enum.TryParse(typeof(Tricking_LibiraryConstants.File.FileType), type, true, out object fileType);
        var mime = (isParsed, fileType) switch
        {
            (true, Tricking_LibiraryConstants.File.FileType.Video) => $"video/{Tricking_LibiraryConstants.File.Mimes.VideoMime}",
            (true, Tricking_LibiraryConstants.File.FileType.Image) => $"image/{Tricking_LibiraryConstants.File.Mimes.ImageMime}",
            _ => null
        };

        if (string.IsNullOrEmpty(mime)) return BadRequest();

        var filePath = fileManager.GeneratePath((Tricking_LibiraryConstants.File.FileType)fileType,file);
        if (string.IsNullOrWhiteSpace(filePath)) return BadRequest();
        return new FileStreamResult(new FileStream(filePath, FileMode.Open, FileAccess.Read), mime);
    }
    [HttpPost]
    public Task<string> UploadFile(IFormFile video)
    {
        string fileName = Tricking_LibiraryConstants.File.Actions.GenerateFileName(Tricking_LibiraryConstants.File.Prefixes.TempPrifex,
            Tricking_LibiraryConstants.File.Mimes.VideoMime);
        return fileManager.SaveFile(video,Tricking_LibiraryConstants.File.FileType.Video,fileName);
    }
    [HttpDelete("{fileName}")]
    public IActionResult DeleteTemporaryFile(string fileName)
    {
        if (fileManager.CheckIsTemporary(fileName) is false) return BadRequest();
        if (fileManager.CheckFileIsExist(fileName) is false) return NoContent();
        fileManager.DeleteFile(fileName);
        return Ok();
    }
    #endregion
}
