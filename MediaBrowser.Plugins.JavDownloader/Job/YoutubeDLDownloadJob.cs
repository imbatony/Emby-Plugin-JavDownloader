// -----------------------------------------------------------------------
// <copyright file="YoutubeDLDownloadJob.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Job
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="YoutubeDLDownloadJob" />.
    /// </summary>
    public class YoutubeDLDownloadJob : IJob
    {
        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => "youtube-dl";

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url
        {
            get => this.Key; set
            {
                this.Url = value;
            }
        }

        /// <summary>
        /// The BuildExtra.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, string}"/>.</returns>
        protected override Dictionary<string, string> BuildExtra()
        {
            return default;
        }

        /// <summary>
        /// The FromExtra.
        /// </summary>
        /// <param name="extra">The extra<see cref="Dictionary{string, string}"/>.</param>
        protected override void FromExtra(Dictionary<string, string> extra)
        {
        }
    }
}
