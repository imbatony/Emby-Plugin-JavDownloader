// -----------------------------------------------------------------------
// <copyright file="SimpleMedia.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System;
    using System.Collections.Generic;
    using MediaBrowser.Plugins.JavDownloader.Job;

    /// <summary>
    /// Defines the <see cref="SimpleMedia" />.
    /// </summary>
    public class SimpleMedia : IMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMedia"/> class.
        /// </summary>
        public SimpleMedia()
        {
            Extras = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Num.
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the Part.
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// Gets or sets the Videos.
        /// </summary>
        public List<VideoInfo> Videos { get; set; }

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
            return new DownloadJob
            {
                Num = this.Num,
                Status = 0,
                Videos = this.Videos,
                Modified = DateTime.UtcNow,
                Created = DateTime.UtcNow
            };
        }
    }
}
