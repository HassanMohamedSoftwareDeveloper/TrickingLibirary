using Microsoft.Extensions.Options;
using TrickingLibirary.Api.Helpers;
using TrickingLibirary.Api.Settings;

namespace TrickingLibirary.Api.BackgroundServices.VideoEditing;

public class FileManagerLocal : IFileManager
{
    #region Fields :
    private static string TempPrifex => Tricking_LibiraryConstants.File.Prefixes.TempPrifex;
    private readonly IWebHostEnvironment env;
    private readonly IOptionsMonitor<FileSettings> fileSettingsMonitor;
    #endregion

    #region CTORS :
    public FileManagerLocal(IWebHostEnvironment env, IOptionsMonitor<FileSettings> fileSettingsMonitor)
    {
        this.env = env;
        this.fileSettingsMonitor = fileSettingsMonitor;
    }
    #endregion

    #region Methods :
    public string GetFFMPEGPath() => Path.Combine(env.ContentRootPath, "FFMPEG", "ffmpeg.exe");
    public string GetFileUrl(string fileName, Tricking_LibiraryConstants.File.FileType fileType)
    {
        var settings = fileSettingsMonitor.CurrentValue;
        return fileType switch
        {
            Tricking_LibiraryConstants.File.FileType.Image => $"{settings.ImageUrl}/{fileName}",
            Tricking_LibiraryConstants.File.FileType.Video => $"{settings.VideoUrl}/{fileName}",
            _ => throw new ArgumentException(null, nameof(fileType))
        };
    }
    public bool CheckIsTemporary(string fileName) => fileName.StartsWith(TempPrifex);
    public bool CheckFileIsExist(string filePath) => File.Exists(filePath);
    public string GeneratePath(Tricking_LibiraryConstants.File.FileType fileType, string fileName)
    => Path.Combine(GenerateDirectoryPath(fileType), fileName);
    public void DeleteFile(string filePath)
    {
        if (CheckFileIsExist(filePath))
            File.Delete(filePath);
    }
    public async Task<string> SaveFile(IFormFile file, Tricking_LibiraryConstants.File.FileType fileType, string fileName)
    {
        var savePath = GeneratePath(fileType, fileName);
        await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(fileStream);
        }
        return fileName;
    }
    #endregion

    #region Helpers :
    private string GenerateDirectoryPath(Tricking_LibiraryConstants.File.FileType fileType)
    {
        var directoryPath = Path.Combine(env.WebRootPath,
              fileType == Tricking_LibiraryConstants.File.FileType.Video ?
              Tricking_LibiraryConstants.File.Directories.VideoDirectory :
              Tricking_LibiraryConstants.File.Directories.ImageDirectory
              );
        if (Directory.Exists(directoryPath) is false)
            Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }
    #endregion
}
