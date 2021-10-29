namespace MediaBrowser.Plugins.JavDownloader.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Downloader;
    using LiteDB;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Data;

    /// <summary>
    /// Defines the <see cref="JavResolveTask" />.
    /// </summary>
    public class JavDownloadTask : IScheduledTask, IConfigurableScheduledTask
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the jobs.
        /// </summary>
        private readonly ILiteCollection<Job> jobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavDownloadTask"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public JavDownloadTask(
            ILogManager logManager
            )
        {
            logger = logManager.GetLogger("JavDownloadTask");
            this.jobs = Plugin.Instance.DB.Jobs;
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => Plugin.NAME + ": 后台下载任务";

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public string Key => Plugin.NAME + "-JavDownloadSingleTask";

        /// <summary>
        /// Gets the Description.
        /// </summary>
        public string Description => Plugin.NAME + (total == 0 ? "下载任务(已停止)" : $": 下载任务({finished}/{total})(正在下载{curNum})");

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
        public bool IsEnabled => Plugin.Instance.Configuration.EnableDownloadTask;

        /// <summary>
        /// Gets a value indicating whether IsLogged.
        /// </summary>
        public bool IsLogged => true;

        /// <summary>
        /// Defines the _currentDownloadService.
        /// </summary>
        private DownloadService _currentDownloadService;

        /// <summary>
        /// Defines the progress.
        /// </summary>
        private IProgress<double> progress;

        /// <summary>
        /// Defines the total.
        /// </summary>
        private int total;

        /// <summary>
        /// Defines the finished.
        /// </summary>
        private int finished;

        /// <summary>
        /// Defines the curNum.
        /// </summary>
        private string curNum;

        /// <summary>
        /// Defines the currentPercentage.
        /// </summary>
        private double currentPercentage;

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
            progress.Report(0);
            if (!Plugin.Instance.Configuration.EnableDownloadTask)
            {
                logger.Info($"Skip Download...");
                progress.Report(100);
                return;
            }
            var now = DateTime.UtcNow.AddDays(-1);
            var jobs = this.jobs.Find(j => j.Created >= now && j.Type == "download" && j.Status == 0, 0, 10);
            if (!jobs.Any())
            {
                logger.Info($"No file need to download...");
                progress.Report(100);
                return;
            }
            var needDownload = jobs.Select(e => new DownloadItem
            {
                Num = e.Num,
                Videos = e.Videos,
                FolderPath = Plugin.Instance.Configuration.DownloadTargetPath,
                FileName = $"{e.Num}.mp4"
            }).ToList();
            total = needDownload.Count();
            finished = 0;
            logger.Info($"{total} files need Download...");
            await DownloadAll(needDownload);
            progress.Report(100);
            total = 0;
            finished = 0;
        }

        /// <summary>
        /// The DownloadAll.
        /// </summary>
        /// <param name="downloadList">The downloadList<see cref="IEnumerable{DownloadItem}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task DownloadAll(IEnumerable<DownloadItem> downloadList)
        {
            foreach (DownloadItem downloadItem in downloadList)
            {
                curNum = downloadItem.Num;
                // begin download from url
                DownloadService ds = await DownloadFile(downloadItem).ConfigureAwait(false);

                // clear download to order new of one
                ds.Clear();
            }
        }

        /// <summary>
        /// The DownloadFile.
        /// </summary>
        /// <param name="downloadItem">The downloadItem<see cref="DownloadItem"/>.</param>
        /// <returns>The <see cref="Task{DownloadService}"/>.</returns>
        private async Task<DownloadService> DownloadFile(DownloadItem downloadItem)
        {
            _currentDownloadService = new DownloadService(GetDownloadConfiguration());
            _currentDownloadService.DownloadProgressChanged += OnDownloadProgressChanged;
            _currentDownloadService.DownloadFileCompleted += OnDownloadFileCompleted;
            _currentDownloadService.DownloadStarted += OnDownloadStarted;

            if(downloadItem.Videos.Count == 1)
            {
                await _currentDownloadService.DownloadFileTaskAsync(downloadItem.Videos[0].Url, Path.Combine(downloadItem.FolderPath, downloadItem.FileName)).ConfigureAwait(false);
            }

            else
            {
                foreach(var v in downloadItem.Videos)
                {
                    await _currentDownloadService.DownloadFileTaskAsync(v.Url, Path.Combine(Path.GetTempPath(), $"{downloadItem.Num}-{v.Part}.mp4")).ConfigureAwait(false);
                }
            }

            return _currentDownloadService;
        }

        /// <summary>
        /// The OnDownloadStarted.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DownloadStartedEventArgs"/>.</param>
        private void OnDownloadStarted(object sender, DownloadStartedEventArgs e)
        {
            logger.Info($"{e.FileName} started");
        }

        /// <summary>
        /// The OnDownloadFileCompleted.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="args">The args<see cref="AsyncCompletedEventArgs"/>.</param>
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs args)
        {
            logger.Info($"{curNum} done");
            var job = this.jobs.FindOne(e => e.Num == curNum);
            job.Status = 1;
            this.jobs.Update(job);
            finished++;
            this.progress.Report(100 * finished / total);
        }

        /// <summary>
        /// The OnDownloadProgressChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DownloadProgressChangedEventArgs"/>.</param>
        private void OnDownloadProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e)
        {
            var percent = e.ProgressPercentage / total;
            this.currentPercentage = e.ProgressPercentage;
            this.progress.Report(100 * finished / total + percent);
        }

        /// <summary>
        /// The GetDownloadConfiguration.
        /// </summary>
        /// <returns>The <see cref="DownloadConfiguration"/>.</returns>
        private static DownloadConfiguration GetDownloadConfiguration()
        {
            var cookies = new CookieContainer();

            return new DownloadConfiguration
            {
                BufferBlockSize = 10240, // usually, hosts support max to 8000 bytes, default values is 8000
                ChunkCount = 8, // file parts to download, default value is 1
                // MaximumBytesPerSecond = 1024 * 1024 * 8, // download speed limited to 1MB/s, default values is zero or unlimited
                MaxTryAgainOnFailover = int.MaxValue, // the maximum number of times to fail
                OnTheFlyDownload = false, // caching in-memory or not? default values is true
                ParallelDownload = true, // download parts of file as parallel or not. Default value is false
                Timeout = 1000, // timeout (millisecond) per stream block reader, default values is 1000
                RequestConfiguration = {
                    // config and customize request headers
                    Accept = "*/*",
                    CookieContainer = cookies,
                    Headers = new WebHeaderCollection(), // { Add your custom headers }
                    KeepAlive = true,
                    ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                    UseDefaultCredentials = false,
                    UserAgent = $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36"
                }
            };
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
                IntervalTicks = (long)TimeSpan.FromMinutes(1).TotalMilliseconds
            };
            return new[] { t };
        }
    }
}
