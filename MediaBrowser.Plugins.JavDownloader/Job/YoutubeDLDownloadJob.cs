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
        /// Defines the JobType.
        /// </summary>
        public static readonly string JobType = "youtube-dl";

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public override string Type => JobType;

        /// <summary>
        /// Gets or sets the DisplayId.
        /// </summary>
        public string DisplayId { get; set; }

        /// <summary>
        /// Gets or sets the ExtractorKey.
        /// </summary>
        public string ExtractorKey { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the Key.
        /// </summary>
        public override string Key => $"{ExtractorKey}_{DisplayId}";

        /// <summary>
        /// The BuildExtra.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, string}"/>.</returns>
        protected override Dictionary<string, string> BuildExtra()
        {
            return new Dictionary<string, string>
            {
                { "DisplayId",this.DisplayId},
                { "ExtractorKey",this.ExtractorKey},
                { "Url",this.Url},
            };
        }

        /// <summary>
        /// The FromExtra.
        /// </summary>
        /// <param name="extra">The extra<see cref="Dictionary{string, string}"/>.</param>
        protected override void FromExtra(Dictionary<string, string> extra)
        {
            this.DisplayId = extra["DisplayId"];
            this.ExtractorKey = extra["ExtractorKey"];
            this.Url = extra["Url"];
        }
    }
}
