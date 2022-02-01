using System.Diagnostics;
using System.Threading.Channels;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.BackgroundServices.VideoEditing;

public class VideoEditingBackgroundService : BackgroundService
{
    #region Fields :
    private readonly IWebHostEnvironment env;
    private readonly ILogger<VideoEditingBackgroundService> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly VideoManager videoManager;
    private readonly ChannelReader<EditVideoMessage> channelReader;
    #endregion

    #region CTORS :
    public VideoEditingBackgroundService(IWebHostEnvironment env, Channel<EditVideoMessage> channel,
        ILogger<VideoEditingBackgroundService> logger, IServiceProvider serviceProvider, VideoManager videoManager)
    {
        this.env = env;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.videoManager = videoManager;
        channelReader = channel.Reader;
    }
    #endregion

    #region Methods :
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channelReader.WaitToReadAsync(stoppingToken))
        {
            var message = await channelReader.ReadAsync(stoppingToken);

            var inputPath = videoManager.GenerateTemporarySavePath(message.Input);
            var outputConvertedName = videoManager.GenerateConvertedFileName();
            var outputThumbnailName = videoManager.GenerateThumbnailFileName();
            var outputConvertedPath = videoManager.GenerateTemporarySavePath(outputConvertedName);
            var outputThumbnailPath = videoManager.GenerateTemporarySavePath(outputThumbnailName);
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = videoManager.FFMPEGPath,
                    Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputConvertedPath} -ss 00:00:00 -vframes 1 -vf scale=540x380 {outputThumbnailPath}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };
                using var process = new Process { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();

                if (videoManager.CheckTemporaryFileIsExist(outputConvertedName) is false)
                    throw new Exception("FFMPEG failed to generate converted video");

                if (videoManager.CheckTemporaryFileIsExist(outputThumbnailName) is false)
                    throw new Exception("FFMPEG failed to generate thumbnail");

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
                var submission = dbContext.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));
                submission.Video = new Domain.Entities.Video
                {
                    VideoLink = outputConvertedName,
                    ThumbLink = outputThumbnailName,
                };
                submission.VideoProcessed = true;

                await dbContext.SaveChangesAsync(stoppingToken);
                logger.LogInformation("Success");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Video Processig Failed for {0}", message.Input);

                videoManager.DeleteTemporaryFile(outputConvertedName);
                videoManager.DeleteTemporaryFile(outputThumbnailName);
            }
            finally
            {
                videoManager.DeleteTemporaryFile(message.Input);
            }
        }
    }
    #endregion
}
