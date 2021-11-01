// -----------------------------------------------------------------------
// <copyright file="DownloadJob.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Job
{
    using System.Collections.Generic;
    using MediaBrowser.Plugins.JavDownloader.Media;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines the <see cref="Job" />.
    /// </summary>
    public class DownloadJob : IJob
    {
        /// <summary>
        /// Gets or sets the Videos.
        /// </summary>
        public List<VideoInfo> Videos { get; set; }

        /// <summary>
        /// Gets or sets the Num.
        /// </summary>
        public string Num
        {
            get => this.Key; set { this.Key = value; }
        }

        public override string Type => "download";

        /// <summary>
        /// The BuildExtra.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, string}"/>.</returns>
        protected override Dictionary<string, string> BuildExtra()
        {
            var dict = new Dictionary<string, string>();
            dict["Videos"] = JsonConvert.SerializeObject(this.Videos);
            return dict;
        }

        /// <summary>
        /// The FromExtra.
        /// </summary>
        /// <param name="extra">The extra<see cref="Dictionary{string, string}"/>.</param>
        protected override void FromExtra(Dictionary<string, string> extra)
        {
            if (extra.TryGetValue("Videos", out var videos))
            {
                this.Videos = JsonConvert.DeserializeObject<List<VideoInfo>>(videos);
            }
        }
    }
}
