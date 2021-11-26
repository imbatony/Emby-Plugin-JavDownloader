// -----------------------------------------------------------------------
// <copyright file="YoutubeDLExtractor.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors
{
    using JavDownloader.Core.Configuration;
    using JavDownloader.Core.Extractor;
    using JavDownloader.Core.Http;
    using JavDownloader.Core.Logger;
    using System.Threading.Tasks;
    using YoutubeDLSharp;
    using YoutubeDLSharp.Metadata;
    using YoutubeDLSharp.Options;

    /// <summary>
    /// Defines the <see cref="YoutubeDLExtractor" />.
    /// </summary>
    public class YoutubeDLExtractor : InfoExtractor
    {
        private readonly YoutubeDL ytdl;
        public const string ffmpegPathKey = "ffmpege";
        public const string youtubedlPathKey = "youtube-dl";

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeDLExtractor"/> class.
        /// </summary>
        /// <param name="configProvider">The configProvider<see cref="IConfigurationProvider"/>.</param>
        /// <param name="clientEx">The clientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public YoutubeDLExtractor(IConfigurationProvider configProvider, IHttpClientEx clientEx, ILogger logger) : base(configProvider, clientEx, logger)
        {
            this.ytdl = new YoutubeDL
            {
                FFmpegPath = configProvider.GetConfig(ffmpegPathKey),
                YoutubeDLPath = configProvider.GetConfig(youtubedlPathKey)
            };
        }

        /// <summary>
        /// The Extractor.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="VideoData"/>.</returns>
        public override async Task<VideoData> Extractor(string url)
        {
            var result = await ytdl.RunVideoDataFetch(url);
            if (result.Success)
            {
                return result.Data;
            }
            else
            {
                logger.Error($"fail to fetch data for {url}, output is ");
                logger.Error(string.Join("\n", result.ErrorOutput));
                return default;
            }
        }

        /// <summary>
        /// The Support.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Support(string url)
        {
            var optionSet = OptionSet.FromString(new string[] { "-e" });
            var result = ytdl.RunWithOptions(new string[] { url }, optionSet, default).Result;
            return result.Success;
        }
    }
}
