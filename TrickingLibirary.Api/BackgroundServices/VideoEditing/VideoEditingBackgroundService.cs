using System.Diagnostics;
using System.Threading.Channels;
using TrickingLibirary.Api.Helpers;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.BackgroundServices.VideoEditing;

public class VideoEditingBackgroundService : BackgroundService
{
    #region Fields :
    private readonly IWebHostEnvironment env;
    private readonly ILogger<VideoEditingBackgroundService> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly IFileManager fileManager;
    private readonly ChannelReader<EditVideoMessage> channelReader;
    #endregion

    #region CTORS :
    public VideoEditingBackgroundService(IWebHostEnvironment env, Channel<EditVideoMessage> channel,
        ILogger<VideoEditingBackgroundService> logger, IServiceProvider serviceProvider, IFileManager fileManager)
    {
        this.env = env;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.fileManager = fileManager;
        channelReader = channel.Reader;
    }
    #endregion

    #region Methods :
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channelReader.WaitToReadAsync(stoppingToken))
        {
            var message = await channelReader.ReadAsync(stoppingToken);

            var inputPath = fileManager.GeneratePath(Tricking_LibiraryConstants.File.FileType.Video, message.Input);
            var outputConvertedName = Tricking_LibiraryConstants.File.Actions
                .GenerateFileName(Tricking_LibiraryConstants.File.Prefixes.ConvertedPrifex,
                Tricking_LibiraryConstants.File.Mimes.VideoMime);

            var outputThumbnailName = Tricking_LibiraryConstants.File.Actions
                .GenerateFileName(Tricking_LibiraryConstants.File.Prefixes.ThumbnailPrifex,
                Tricking_LibiraryConstants.File.Mimes.ImageMime);
            var outputConvertedPath = fileManager.GeneratePath(Tricking_LibiraryConstants.File.FileType.Video, outputConvertedName);
            var outputThumbnailPath = fileManager.GeneratePath(Tricking_LibiraryConstants.File.FileType.Image, outputThumbnailName);
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileManager.GetFFMPEGPath(),
                    Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputConvertedPath} -ss 00:00:00 -vframes 1 -vf scale=540x380 {outputThumbnailPath}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };
                using var process = new Process { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();

                if (fileManager.CheckFileIsExist(outputConvertedPath) is false)
                    throw new Exception("FFMPEG failed to generate converted video");

                if (fileManager.CheckFileIsExist(outputThumbnailPath) is false)
                    throw new Exception("FFMPEG failed to generate thumbnail");

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
                var submission = dbContext.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));
                submission.Video = new Domain.Entities.Video
                {
                    VideoLink =fileManager.GetFileUrl(outputConvertedName,Tricking_LibiraryConstants.File.FileType.Video),
                    ThumbLink =fileManager.GetFileUrl(outputThumbnailName,Tricking_LibiraryConstants.File.FileType.Image),
                };
                submission.VideoProcessed = true;

                await dbContext.SaveChangesAsync(stoppingToken);
                logger.LogInformation("Success");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Video Processig Failed for {input}", message.Input);

                fileManager.DeleteFile(outputConvertedPath);
                fileManager.DeleteFile(outputThumbnailPath);
            }
            finally
            {
                fileManager.DeleteFile(inputPath);
            }
        }
    }
    #endregion
}
