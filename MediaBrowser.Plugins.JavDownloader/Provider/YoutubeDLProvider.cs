// -----------------------------------------------------------------------
// <copyright file="YoutubeDLProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Job;
    using MediaBrowser.Plugins.JavDownloader.Media;
    using YoutubeDLSharp;

    /// <summary>
    /// Defines the <see cref="YoutubeDLProvider" />.
    /// </summary>
    public class YoutubeDLProvider : IJavProvider
    {
        /// <summary>
        /// Defines the ytdl.
        /// </summary>
        private readonly YoutubeDL ytdl;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeDLProvider"/> class.
        /// </summary>
        /// <param name="youtubedlPath">The youtubedlPath<see cref="string"/>.</param>
        /// <param name="ffmpegPath">The ffmpegPath<see cref="string"/>.</param>
        public YoutubeDLProvider(string youtubedlPath, string ffmpegPath)
        {
            this.ytdl = new YoutubeDL();
            this.ytdl.FFmpegPath = ffmpegPath;
            this.ytdl.YoutubeDLPath = youtubedlPath;
        }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public string Type => "YoutubeDL";

        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public Task<List<IMedia>> GetTodayPopular()
        {
            return Task.FromResult(new List<IMedia>());
        }

        /// <summary>
        /// The Match.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Match(string url)
        {
            return url.IsWebUrl();
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public async Task<List<IMedia>> Resolve(string url)
        {
            var result = await ytdl.RunVideoDataFetch(url);
            if (result.Success)
            {
                return new List<IMedia>
                {
                   new YoutubeDLMedia(result.Data)
                };
            }
            return new List<IMedia>();
        }
    }
}
