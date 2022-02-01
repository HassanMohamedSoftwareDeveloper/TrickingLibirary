namespace TrickingLibirary.Api.BackgroundServices.VideoEditing;

public class VideoManager
{
    #region Consts :
    private const string Directory = "videos";
    private const string TempPrifex = "temp_";
    private const string ConvertedPrifex = "c";
    private const string ThumbnailPrifex = "t";
    private const string ConvertedVideoExtension = ".mp4";
    private const string ThumbnailExtension = ".png";
    #endregion
    #region Fields :
    private readonly IWebHostEnvironment env;
    #endregion
    #region PROPS :
    public string WorkingDirectoryPath => Path.Combine(env.WebRootPath, Directory);
    #endregion

    #region CTORS :
    public VideoManager(IWebHostEnvironment env)
    {
        this.env = env;
    }
    #endregion

    #region Methods :
    public bool CheckIsTemporary(string fileName) => fileName.StartsWith(TempPrifex);
    public bool CheckTemporaryVideoIsExist(string fileName) => File.Exists(GenerateTemporarySavePath(fileName));
    public void DeleteTemporaryVideo(string fileName) => File.Delete(GenerateTemporarySavePath(fileName));
    public string GenerateDevVideoPath(string fileName) => env.IsDevelopment() ? Path.Combine(WorkingDirectoryPath, fileName) : null;
    public string GenerateConvertedFileName() => string.Concat(ConvertedPrifex, DateTime.Now.Ticks, ConvertedVideoExtension);
    public string GenerateThumbnailFileName() => string.Concat(ThumbnailPrifex, DateTime.Now.Ticks, ThumbnailExtension);

    public string GenerateTemporarySavePath(string fileName) => Path.Combine(WorkingDirectoryPath, fileName);
    public async Task<string> SaveTemporaryVideo(IFormFile video)
    {
        var fileName = string.Concat(TempPrifex, DateTime.Now.Ticks, Path.GetExtension(video.FileName));
        var savePath = GenerateTemporarySavePath(fileName);
        await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
        {
            await video.CopyToAsync(fileStream);
        }
        return fileName;
    }

    #endregion
    #region Helpers :
    private string GetMime(string fileName) => fileName.Split('.').Last();
    #endregion
}
