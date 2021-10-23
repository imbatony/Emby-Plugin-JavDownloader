namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SimpleMedia :IMedia
    {
        /// <summary>
        /// Gets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the Num.
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Gets the Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the Provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets the Videos.
        /// </summary>
        public List<JavVideo> Videos { get; set; }
    }
}
