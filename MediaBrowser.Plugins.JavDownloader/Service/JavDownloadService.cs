namespace MediaBrowser.Plugins.JavDownloader.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiteDB;
    using MediaBrowser.Common.Extensions;
    using MediaBrowser.Controller.Net;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Services;
    using MediaBrowser.Model.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Provider;

    /// <summary>
    /// Defines the <see cref="ResolveReqeust" />.
    /// </summary>
    [Route("/emby/Plugins/JavDownloader/Download", "GET")]
    public class DownloadReqeust : IReturn<List<DownloadItem>>
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="JavResolveService" />.
    /// </summary>
    public class JavDownloaderService : IService, IRequiresRequest
    {
        /// <summary>
        /// Defines the resultFactory.
        /// </summary>
        private readonly IHttpResultFactory resultFactory;

        /// <summary>
        /// Defines the taskManager.
        /// </summary>
        private readonly ITaskManager taskManager;

        /// <summary>
        /// Defines the jobs.
        /// </summary>
        private readonly ILiteCollection<Job> jobs;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the javProvider.
        /// </summary>
        private readonly CompositeJavProvider javProvider;

        /// <summary>
        /// Gets or sets the request context.......
        /// </summary>
        public IRequest Request { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavDownloaderService"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        /// <param name="resultFactory">The resultFactory<see cref="IHttpResultFactory"/>.</param>
        /// <param name="taskManager">The taskManager<see cref="ITaskManager"/>.</param>
        public JavDownloaderService(
            ILogManager logManager,
            IHttpResultFactory resultFactory,
            ITaskManager taskManager
            )
        {
            this.logger = logManager.GetLogger("JavResolveService");
            this.javProvider = Plugin.Instance.javProvider;
            this.resultFactory = resultFactory;
            this.taskManager = taskManager;
            this.jobs = Plugin.Instance.DB.Jobs;
        }

        /// <summary>
        /// The DoGet.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private async Task<List<DownloadItem>> DoGet(string url)
        {
            logger.Info($"{url}");

            if (string.IsNullOrEmpty(url) || !url.IsWebUrl())
                throw new ResourceNotFoundException();

            var meidas = await javProvider.Resolve(url);
            var downloads = DownloadItem.FromMedias(meidas, Plugin.Instance.Configuration.DownloadTargetPath);
            var now = DateTime.UtcNow.AddDays(-10);
            foreach (var item in downloads)
            {
                if (jobs.Exists(e => e.Num == item.Num && e.Created > now))
                {
                    item.Extras["jobId"] = jobs.FindOne(e => e.Num == item.Num && e.Created > now).id.ToString();
                    item.Extras["exists"] = "true";
                }

                else
                {
                    var job = jobs.Insert(new Job
                    {
                        Type = "download",
                        Num = item.Num,
                        Status = 0,
                        Videos = item.Videos,
                        Modified = DateTime.UtcNow,
                        Created = DateTime.UtcNow,
                        Quality = item.Quality,
                        FileType = item.FileType
                    });
                    item.Extras["jobId"] = job.AsObjectId.ToString();
                    item.Extras["exists"] = "false";
                }
            }
            return downloads;
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="request">The request<see cref="ResolveReqeust"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public List<DownloadItem> Get(DownloadReqeust request)
        {
            return Task.Run(() => DoGet(request.Url)).GetAwaiter().GetResult();
        }
    }
}
