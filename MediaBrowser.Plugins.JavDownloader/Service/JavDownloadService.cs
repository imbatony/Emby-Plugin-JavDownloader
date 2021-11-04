// -----------------------------------------------------------------------
// <copyright file="JavDownloadService.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediaBrowser.Common.Extensions;
    using MediaBrowser.Controller.Net;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Services;
    using MediaBrowser.Model.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Job;
    using MediaBrowser.Plugins.JavDownloader.Provider;

    /// <summary>
    /// Defines the <see cref="ResolveReqeust" />.
    /// </summary>
    [Route("/emby/Plugins/JavDownloader/Download", "GET")]
    public class DownloadReqeust : IReturn<List<IJob>>
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
        /// Defines the jobs.
        /// </summary>
        private readonly JobRepository jobs;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the javProvider.
        /// </summary>
        private readonly CompositeJavProvider javProvider;

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        public IRequest Request { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavDownloaderService"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        /// <param name="resultFactory">The resultFactory<see cref="IHttpResultFactory"/>.</param>
        /// <param name="taskManager">The taskManager<see cref="ITaskManager"/>.</param>
        public JavDownloaderService(ILogManager logManager)
        {
            this.logger = logManager.GetLogger("JavResolveService");
            this.javProvider = Plugin.Instance.javProvider;
            this.jobs = Plugin.Instance.JobRepository;
        }

        /// <summary>
        /// The DoGet.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private async Task<List<IJob>> DoGet(string url)
        {
            logger.Info($"{url}");

            if (string.IsNullOrEmpty(url) || !url.IsWebUrl())
                throw new ResourceNotFoundException();

            var meidas = await javProvider.Resolve(url);
            var downloads = meidas.Select(e => e.CreateDownloadJob()).ToList();
            var now = DateTime.UtcNow.AddDays(-10);
            foreach (var item in downloads)
            {
                if (!jobs.IsExist(item))
                {
                    var job = jobs.UpsertJob(item);
                }
            }
            return downloads;
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="request">The request<see cref="ResolveReqeust"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public List<IJob> Get(DownloadReqeust request)
        {
            return Task.Run(() => DoGet(request.Url)).GetAwaiter().GetResult();
        }
    }
}
