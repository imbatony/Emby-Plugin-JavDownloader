// -----------------------------------------------------------------------
// <copyright file="YoutubeDLMedia.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System.Collections.Generic;
    using MediaBrowser.Plugins.JavDownloader.Job;
    using YoutubeDLSharp.Metadata;

    /// <summary>
    /// Defines the <see cref="YoutubeDLMedia" />.
    /// </summary>
    public class YoutubeDLMedia : IMedia
    {
        /// <summary>
        /// Gets the VideoData
        /// Gets or sets the VideoData
        /// Defines the data....
        /// </summary>
        public VideoData VideoData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeDLMedia"/> class.
        /// </summary>
        /// <param name="data">The data<see cref="VideoData"/>.</param>
        public YoutubeDLMedia(VideoData data)
        {
            Extras = new Dictionary<string, string>();
            this.VideoData = data;
        }

        /// <summary>
        /// Gets the Url.
        /// </summary>
        public string Url => this.VideoData.WebpageUrl;

        /// <summary>
        /// Gets the Title
        /// Gets or sets the Title....
        /// </summary>
        public string Title { get => this.VideoData.Title; }

        /// <summary>
        /// Gets the Provider
        /// Gets or sets the Provider....
        /// </summary>
        public string Provider { get => "Youtube-dl"; }

        /// <summary>
        /// Gets or sets the Extras.
        /// </summary>
        public Dictionary<string, string> Extras { get; set; }

        /// <summary>
        /// The CreateDownloadJob.
        /// </summary>
        /// <returns>The <see cref="Job"/>.</returns>
        public IJob CreateDownloadJob()
        {
            return new YoutubeDLDownloadJob
            {
                Url = this.Url,
                ExtractorKey = this.VideoData.ExtractorKey,
                DisplayId = this.VideoData.DisplayID
            };
        }
    }
}
