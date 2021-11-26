// -----------------------------------------------------------------------
// <copyright file="InfoExtractor.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Core.Extractor
{
    using JavDownloader.Core.Configuration;
    using JavDownloader.Core.Http;
    using JavDownloader.Core.Logger;
    using System.Threading.Tasks;
    using YoutubeDLSharp.Metadata;

    /// <summary>
    /// Defines the <see cref="InfoExtractor" />.
    /// </summary>
    public abstract class InfoExtractor
    {
        protected readonly IConfigurationProvider configProvider;

        /// <summary>
        /// Defines the clientEx.
        /// </summary>
        protected readonly IHttpClientEx clientEx;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoExtractor"/> class.
        /// </summary>
        /// <param name="configProvider">The configProvider<see cref="IConfigurationProvider"/>.</param>
        /// <param name="clientEx">The clientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public InfoExtractor(IConfigurationProvider configProvider, IHttpClientEx clientEx, ILogger logger)
        {
            this.configProvider = configProvider;
            this.clientEx = clientEx;
            this.logger = logger;
        }

        /// <summary>
        /// The Extractor.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="VideoData"/>.</returns>
        public abstract Task<VideoData> Extractor(string url);

        /// <summary>
        /// The Support.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public abstract bool Support(string url);
    }
}
