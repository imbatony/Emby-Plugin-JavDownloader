namespace MediaBrowser.Plugins.JavDownloader.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Job;
    using YoutubeDLSharp;

    /// <summary>
    /// Defines the <see cref="YoutubeDownloadTask" />.
    /// </summary>
    public class YoutubeDownloadTask : IScheduledTask, IConfigurableScheduledTask
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the jobRepository.
        /// </summary>
        private readonly JobRepository jobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeDownloadTask"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public YoutubeDownloadTask(
            ILogManager logManager
            )
        {
            logger = logManager.GetLogger("YoutubeDownloadTask");
            this.jobRepository = Plugin.Instance.JobRepository;
            this.ytdl = new YoutubeDL();
            this.ytdl.FFmpegPath = Plugin.Instance.Configuration.FFmpegPath;
            this.ytdl.YoutubeDLPath = Plugin.Instance.Configuration.YoutubeDLPath;
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => Plugin.NAME + ": youtube后台下载任务";

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public string Key => Plugin.NAME + "-YoutubeDownloadTask";

        /// <summary>
        /// Gets the Description.
        /// </summary>
        public string Description => Plugin.NAME + (total == 0 ? "下载任务(已停止)" : $": 下载任务({finished}/{total})(正在下载{curKey})");

        /// <summary>
        /// Gets the Category.
        /// </summary>
        public string Category => Plugin.NAME;

        /// <summary>
        /// Gets a value indicating whether IsHidden.
        /// </summary>
        public bool IsHidden => false;

        /// <summary>
        /// Gets a value indicating whether IsEnabled.
        /// </summary>
        public bool IsEnabled => Plugin.Instance.Configuration.EnableDownloadTask && Plugin.Instance.Configuration.EnableYoutubeDL;

        /// <summary>
        /// Gets a value indicating whether IsLogged.
        /// </summary>
        public bool IsLogged => true;

        /// <summary>
        /// Defines the total.
        /// </summary>
        private int total;

        /// <summary>
        /// Defines the finished.
        /// </summary>
        private int finished;

        /// <summary>
        /// Defines the curKey.
        /// </summary>
        private string curKey;

        /// <summary>
        /// Defines the progress.
        /// </summary>
        private IProgress<double> progress;

        /// <summary>
        /// Defines the ytdl.
        /// </summary>
        private readonly YoutubeDL ytdl;

        /// <summary>
        /// The Execute.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <param name="progress">The progress<see cref="IProgress{double}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            logger.Info($"Running...");
            this.progress = progress;
            this.progress.Report(0);
            if (!Plugin.Instance.Configuration.EnableDownloadTask)
            {
                logger.Info($"Skip Download...");
                progress.Report(100);
                return;
            }
            var expired = DateTime.UtcNow.AddDays(-1);
            var jobs = this.jobRepository.GetJobs<YoutubeDLDownloadJob>(j => j.Created >= expired && j.Type == YoutubeDLDownloadJob.JobType && j.Status == 0, 0, 10);
            if (!jobs.Any())
            {
                logger.Info($"No file need to download...");
                progress.Report(100);
                return;
            }
            var progressFunc = new Progress<DownloadProgress>(e =>
            {
                var percent = 100 * e.Progress / total;
                this.progress.Report(100 * finished / total + percent);
            });
            total = jobs.Count;
            finished = 0;
            foreach (var d in jobs)
            {
                curKey = d.Key;
                logger.Info($"Downloading {curKey}");
                var target = Path.Combine(Plugin.Instance.Configuration.DownloadTargetPath, d.ExtractorKey);
                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }
                this.ytdl.OutputFolder = target;
                RunResult<string> result = await ytdl.RunVideoDownload(d.Url, progress: progressFunc, ct: cancellationToken).ConfigureAwait(false);
                logger.Info($"result is {result.Success}, data is {result.Data}, error output is {string.Join("\n",result.ErrorOutput)}");
                finished++;
                d.Status = 1;
                this.jobRepository.UpsertJob(d);
            }
            progress.Report(100);
            total = 0;
            finished = 0;
        }

        /// <summary>
        /// The GetDefaultTriggers.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{TaskTriggerInfo}"/>.</returns>
        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            var t = new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerInterval,
                IntervalTicks = (long)TimeSpan.FromMinutes(1).TotalMilliseconds * 10000
            };
            return new[] { t };
        }
    }
}
