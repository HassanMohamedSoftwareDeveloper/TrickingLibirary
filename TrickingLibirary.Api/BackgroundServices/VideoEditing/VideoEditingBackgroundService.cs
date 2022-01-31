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
            try
            {
                var inputPath = videoManager.GenerateTemporarySavePath(message.Input);
                var outputName = videoManager.GenerateConvertedFileName();
                var outputPath = videoManager.GenerateTemporarySavePath(outputName);

                var startInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(env.ContentRootPath, "FFMPEG", "ffmpeg.exe"),
                    Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}",
                    WorkingDirectory = videoManager.WorkingDirectoryPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };
                using var process = new Process { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();

                if (videoManager.CheckTemporaryVideoIsExist(outputName) is false)
                    throw new Exception("FFMPEG failed to generate converted video");

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
                var submission = dbContext.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));
                submission.Video = outputName;
                submission.VideoProcessed = true;

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Video Processig Failed for {0]", message.Input);
            }
            finally
            {
                videoManager.DeleteTemporaryVideo(message.Input);
            }
        }
    }
    #endregion
}
