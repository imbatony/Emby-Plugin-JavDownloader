namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SimpleMedia" />.
    /// </summary>
    public class SimpleMedia : IMedia
    {
        public SimpleMedia()
        {
            Extras = new Dictionary<string, string>();
        }
        /// <summary>
        /// Gets or sets the Url
        /// Gets the Url..
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Num
        /// Gets the Num..
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// Gets the Title..
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Provider
        /// Gets the Provider..
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the Part
        /// Gets the Provider..
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// Gets or sets the Videos
        /// Gets the Videos..
        /// </summary>
        public List<JavVideo> Videos { get; set; }

        /// <summary>
        /// Gets the Extras.
        /// </summary>
        public Dictionary<string, string> Extras { get; set; }
    }
}