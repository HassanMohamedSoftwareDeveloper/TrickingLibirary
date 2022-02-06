using IdentityServer4;

namespace TrickingLibirary.Api.Helpers;

public static class Tricking_LibiraryConstants
{
    public struct Policies
    {
        public const string User = IdentityServerConstants.LocalApi.PolicyName;
        public const string Mod = nameof(Mod);
    }
    public struct IdentityResources
    {
        public const string RoleScope = "role";
    }
    public struct Claims
    {
        public const string Role = "role";
    }
    public struct Roles
    {
        public const string Mod = nameof(Mod);
    }
    public struct File
    {
        #region Enums :
        public enum FileType
        {
            Image,
            Video
        }
        #endregion

        #region Constants :
        public struct Directories
        {

            public const string FileDirectory = "uploadedfiles";
            public const string VideoDirectory = $"{FileDirectory}/videos";
            public const string ImageDirectory = $"{FileDirectory}/images";
        }
        public struct Prefixes
        {

            public const string TempPrifex = "temp_";
            public const string ConvertedPrifex = "c";
            public const string ThumbnailPrifex = "t";
            public const string ProfilePrifex = "p";
        }
        public struct Mimes
        {

            public const string VideoMime = "mp4";
            public const string ImageMime = "jpg";
        }
        #endregion

        #region Actions :
        public struct Actions
        {
            public static string GenerateFileName(string prefix, string mime) => string.Concat(prefix, DateTime.Now.Ticks, ".", mime);
        }
        #endregion
    }
    public struct Providers
    {
        public const string Local = nameof(Local);
        public const string S3 = nameof(S3);
    }
}
