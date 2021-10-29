namespace MediaBrowser.Plugins.JavDownloader.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Common.Extensions;
    using MediaBrowser.Controller.Net;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Services;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Media;
    using MediaBrowser.Plugins.JavDownloader.Provider;

    /// <summary>
    /// Defines the <see cref="ResolveReqeust" />.
    /// </summary>
    [Route("/emby/Plugins/JavDownloader/Resolve", "GET")]
    public class ResolveReqeust : IReturn<List<IMedia>>
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="JavResolveService" />.
    /// </summary>
    public class JavResolveService : IService, IRequiresRequest
    {
        /// <summary>
        /// Defines the resultFactory.
        /// </summary>
        private readonly IHttpResultFactory resultFactory;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the javProvider.
        /// </summary>
        private readonly CompositeJavProvider javProvider;

        /// <summary>
        /// Gets or sets the request context...
        /// </summary>
        public IRequest Request { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavResolveService"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        /// <param name="resultFactory">The resultFactory<see cref="IHttpResultFactory"/>.</param>
        public JavResolveService(
            ILogManager logManager,
            IHttpResultFactory resultFactory
            )
        {
            this.logger = logManager.GetLogger("JavResolveService");
            this.javProvider = Plugin.Instance.javProvider;
            this.resultFactory = resultFactory;

        }

        /// <summary>
        /// The DoGet.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        private async Task<List<IMedia>> DoGet(string url)
        {
            logger.Info($"{url}");
            if (!string.IsNullOrEmpty(url) && !url.IsWebUrl())
                throw new ResourceNotFoundException();

            if (string.IsNullOrEmpty(url))
            {
                return await javProvider.GetTodayPopular();
            }
            else
            {
                return await javProvider.Resolve(url);
            }
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="request">The request<see cref="ResolveReqeust"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public List<IMedia> Get(ResolveReqeust request)
        {
            return Task.Run(() => DoGet(request.Url)).GetAwaiter().GetResult();
        }
    }
}
