// -----------------------------------------------------------------------
// <copyright file="JavResolveTask.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
        /// Defines the jobs.
        /// </summary>
        private readonly JobRepository jobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavResolveTask"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public JavResolveTask(
            ILogManager logManager
            )
        {
            logger = logManager.GetLogger("JavResolveTask");
            this.jobs = Plugin.Instance.JobRepository;
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
            if (!IsEnabled)
            {
                logger.Info($"Skip Download...");
                progress.Report(100);
                return;
            }
            else
            {
                var list = await Plugin.Instance.javProvider.GetTodayPopular();
                logger.Info($"Target Path is {Plugin.Instance.Configuration.DownloadTargetPath}...");
                var downloadList = list.Select(e => e.CreateDownloadJob()).ToList();
                var needDownload = downloadList.Where(e => !jobs.IsExist(e));
                if (needDownload.Count() == 0)
                {
                    logger.Info($"No file need Download...");
                    progress.Report(100);
                    return;
                }

                logger.Info($"{needDownload.Count()} files need Download...");
                foreach (var item in needDownload)
                {
                    jobs.UpsertJob(item);
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
