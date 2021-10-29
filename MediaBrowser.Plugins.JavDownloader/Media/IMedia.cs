// -----------------------------------------------------------------------
// <copyright file="IMedia.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System.Collections.Generic;

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
        /// Gets the Num.
        /// </summary>
        string Num { get; }

        /// <summary>
        /// Gets the Title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the Provider.
        /// </summary>
        string Provider { get; }

        /// <summary>
        /// Gets the Videos.
        /// </summary>
        List<JavVideo> Videos { get; }

        /// <summary>
        /// Gets the Extras.
        /// </summary>
        Dictionary<string, string> Extras{ get; }
    }
}
