namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="AbstractMediaResolver" />.
    /// </summary>
    public abstract class AbstractMediaResolver : IMediaResolver
    {
        /// <summary>
        /// Defines the httpClientEx.
        /// </summary>
        protected readonly IHttpClientEx httpClientEx;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMediaResolver"/> class.
        /// </summary>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public AbstractMediaResolver(IHttpClientEx httpClientEx, ILogger logger)
        {
            this.httpClientEx = httpClientEx;
            this.logger = logger;
        }

        /// <summary>
        /// The GetMedias.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public abstract Task<List<IMedia>> GetMedias(string url);
    }
}
