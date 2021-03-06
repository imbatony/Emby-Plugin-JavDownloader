// -----------------------------------------------------------------------
// <copyright file="VideoQuality.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Media
{
    /// <summary>
    /// Defines the <see cref="VideoQuality" />.
    /// </summary>
    public enum VideoQuality
    {
        /// <summary>
        /// Defines the unknown.
        /// </summary>
        unknown = 0,
        /// <summary>
        /// Defines the 360p.
        /// </summary>
        p360,

        /// <summary>
        /// Defines the p480.
        /// </summary>
        p480,
        /// <summary>
        /// Defines the 720p.
        /// </summary>
        p720,
        /// <summary>
        /// Defines the 1080p.
        /// </summary>
        p1080,
        /// <summary>
        /// Defines the 2k.
        /// </summary>
        p2k,
        /// <summary>
        /// Defines the 4k.
        /// </summary>
        p4k,
        /// <summary>
        /// Defines the over 4k.
        /// </summary>
        over4k
    }

    /// <summary>
    /// Defines the <see cref="VideoQualityParser" />.
    /// </summary>
    public static class VideoQualityParser
    {
        /// <summary>
        /// The Parse.
        /// </summary>
        /// <param name="str">The str<see cref="string"/>.</param>
        /// <returns>The <see cref="VideoQuality"/>.</returns>
        public static VideoQuality Parse(string str)
        {
            if (str == "480p")
            {
                return VideoQuality.p480;
            }
            else if(str == "720p")
            {
                return VideoQuality.p720;
            }
            else if (str == "1080p")
            {
                return VideoQuality.p1080;
            }
            else
            {
                return VideoQuality.unknown;
            }
        }

        public static string ToString(VideoQuality quality)
        {
            var content = "";
            switch (quality)
            {
                case VideoQuality.unknown:
                    content = "U";
                    break;
                case VideoQuality.p360:
                    content = "360P";
                    break;
                case VideoQuality.p480:
                    content = "480P";
                    break;
                case VideoQuality.p720:
                    content = "720P";
                    break;
                case VideoQuality.p1080:
                    content = "1080P";
                    break;
                case VideoQuality.p2k:
                    content = "2K";
                    break;
                case VideoQuality.p4k:
                    content = "4K";
                    break;
                case VideoQuality.over4k:
                    content = "8K";
                    break;
                default:
                    break;
            }
            return content;
        }
    }
}
