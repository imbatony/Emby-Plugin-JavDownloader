// -----------------------------------------------------------------------
// <copyright file="IMedia.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System.Collections.Generic;
    using MediaBrowser.Plugins.JavDownloader.Job;

    /// <summary>
    /// Defines the <see cref="IMedia" />.
    /// </summary>
    public interface IMedia
    {
        /// <summary>
        /// Gets the Url.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Gets the Title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the Provider.
        /// </summary>
        string Provider { get; }

        /// <summary>
        /// Gets the Extras.
        /// </summary>
        Dictionary<string, string> Extras { get; }

        /// <summary>
        /// The CreateDownloadJob.
        /// </summary>
        /// <returns>The <see cref="Job"/>.</returns>
        IJob CreateDownloadJob();
    }
}
