namespace MediaBrowser.Plugins.JavDownloader.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Data;

    /// <summary>
    /// Defines the <see cref="JavResolveTask" />.
    /// </summary>
    public class JavResolveTask : IScheduledTask, IConfigurableScheduledTask
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavResolveTask"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public JavResolveTask(
            ILogManager logManager
            )
        {
            logger = logManager.GetLogger("JavResolveTask");
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => Plugin.NAME + ": 解析任务";

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public string Key => Plugin.NAME + "-JavResolveTask";

        /// <summary>
        /// Gets the Description.
        /// </summary>
        public string Description => Plugin.NAME + ": 解析最新流行任务";

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
        /// The Execute.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <param name="progress">The progress<see cref="IProgress{double}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            logger.Info($"Running...");
            progress.Report(0);
            if (!Plugin.Instance.Configuration.EnableDownloadTask)
            {
                logger.Info($"Skip Download...");
                progress.Report(100);
                return;
            }
            else
            {
                var list = await Plugin.Instance.javProvider.GetTodayPopular();
                logger.Info($"Target Path is {Plugin.Instance.Configuration.DownloadTargetPath}...");
                var downloadList = DownloadItem.FromMedias(list, Plugin.Instance.Configuration.DownloadTargetPath);
                var needDownload = downloadList.Where(e => !Plugin.Instance.DB.Jobs.Exists(f => f.Num == e.Num && f.Type == "download"));
                if (needDownload.Count() == 0)
                {
                    logger.Info($"No file need Download...");
                    progress.Report(100);
                    return;
                }

                logger.Info($"{needDownload.Count()} files need Download...");
                foreach (var item in needDownload)
                {
                    Plugin.Instance.DB.Jobs.Insert(new Job
                    {
                        Type = "download",
                        Num = item.Num,
                        Status = 0,
                        Url = item.Url,
                        Modified = DateTime.UtcNow,
                        Created = DateTime.UtcNow,
                        Quality = item.Quality,
                        FileType = item.FileType
                    });
                }

                progress.Report(100);
            }
        }

        /// <summary>
        /// The GetDefaultTriggers.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{TaskTriggerInfo}"/>.</returns>
        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            var t = new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerDaily,
                TimeOfDayTicks = TimeSpan.FromHours(2).Ticks,
                MaxRuntimeTicks = TimeSpan.FromHours(14).Ticks
            };
            return new[] { t };
        }
    }
}
