// -----------------------------------------------------------------------
// <copyright file="IVideo.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    /// <summary>
    /// Defines the <see cref="JavVideo" />.
    /// </summary>
    public class JavVideo
    {
        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the VideoQuality.
        /// </summary>
        public VideoQuality VideoQuality { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public VideoType Type { get; set; }

        /// <summary>
        /// Gets or sets the Part.
        /// </summary>
        public string Part { get; set; }
    }
}
