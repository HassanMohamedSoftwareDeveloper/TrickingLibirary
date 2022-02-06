using TrickingLibirary.Api.Helpers;

namespace TrickingLibirary.Api.BackgroundServices.VideoEditing;

public interface IFileManager
{
    #region Methods :
    string GetFFMPEGPath();
    string GetFileUrl(string fileName, Tricking_LibiraryConstants.File.FileType fileType);
    bool CheckIsTemporary(string fileName);
    bool CheckFileIsExist(string filePath);
    string GeneratePath(Tricking_LibiraryConstants.File.FileType fileType, string fileName);
    void DeleteFile(string fileName);
    Task<string> SaveFile(IFormFile file, Tricking_LibiraryConstants.File.FileType fileType, string fileName);
    #endregion
}
