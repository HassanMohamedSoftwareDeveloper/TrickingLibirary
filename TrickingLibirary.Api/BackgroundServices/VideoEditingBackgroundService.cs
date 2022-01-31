using System.Diagnostics;
using System.Threading.Channels;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.BackgroundServices;

public class VideoEditingBackgroundService : BackgroundService
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<VideoEditingBackgroundService> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly ChannelReader<EditVideoMessage> channelReader;

    public VideoEditingBackgroundService(IWebHostEnvironment env, Channel<EditVideoMessage> channel,
        ILogger<VideoEditingBackgroundService> logger, IServiceProvider serviceProvider)
    {
        this.env = env;
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.channelReader = channel.Reader;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channelReader.WaitToReadAsync(stoppingToken))
        {
            var message = await channelReader.ReadAsync(stoppingToken);
            try
            {
                string inputPath = Path.Combine(env.WebRootPath,"videos",message.Input);
                string outputName = $"c{DateTime.Now.Ticks}.mp4";
                string outputPath = Path.Combine(env.WebRootPath,"videos", outputName);
                var startInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(env.ContentRootPath, "FFMPEG", "ffmpeg.exe"),
                    Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}",
                    WorkingDirectory=Path.Combine(env.WebRootPath,"videos"),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };
                using var process = new Process { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();

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
        }
    }
}

public class EditVideoMessage
{
    public int SubmissionId { get; set; }
    public string Input { get; set; }
}
