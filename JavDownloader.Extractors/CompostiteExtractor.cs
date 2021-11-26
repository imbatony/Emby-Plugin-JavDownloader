// -----------------------------------------------------------------------
// <copyright file="CompostiteExtractor.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors
{
    using JavDownloader.Core.Configuration;
    using JavDownloader.Core.Extractor;
    using JavDownloader.Core.Http;
    using JavDownloader.Core.Logger;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using YoutubeDLSharp.Metadata;

    /// <summary>
    /// Defines the <see cref="CompostiteExtractor" />.
    /// </summary>
    public class CompostiteExtractor : InfoExtractor
    {
        /// <summary>
        /// Defines the extrators.
        /// </summary>
        private readonly IEnumerable<InfoExtractor> extrators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompostiteExtractor"/> class.
        /// </summary>
        /// <param name="configProvider">The configProvider<see cref="IConfigurationProvider"/>.</param>
        /// <param name="clientEx">The clientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public CompostiteExtractor(IConfigurationProvider configProvider, IHttpClientEx clientEx, ILogger logger) : base(configProvider, clientEx, logger)
        {
            extrators = Assembly.GetAssembly(typeof(CompostiteExtractor))
                .GetTypes()
                .Where(e => e.BaseType == typeof(InfoExtractor) && e != typeof(CompostiteExtractor))
                .Select(e => (InfoExtractor)Activator.CreateInstance(e, clientEx, logger));
        }

        /// <summary>
        /// The Extractor.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="VideoData"/>.</returns>
        public override Task<VideoData> Extractor(string url)
        {
            return extrators
                  .Where(e => e.Support(url))
                  .FirstOrDefault()?.Extractor(url) ?? default;
        }

        /// <summary>
        /// The Support.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Support(string url)
        {
            return extrators.Any(e => e.Support(url));
        }
    }
}
