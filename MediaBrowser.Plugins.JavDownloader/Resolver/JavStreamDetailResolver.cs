// -----------------------------------------------------------------------
// <copyright file="SuperJavProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="JavStreamDetailResolver" />.
    /// </summary>
    public class JavStreamDetailResolver : AbstractMediaResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JavStreamDetailResolver"/> class.
        /// </summary>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public JavStreamDetailResolver(IHttpClientEx httpClientEx, ILogger logger) : base(httpClientEx, logger)
        {
        }

        /// <summary>
        /// The GetMedias.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public override async Task<List<IMedia>> GetMedias(string url)
        {
            var id = url.Substring("https://javstream.top/v/".Length);
            id = id.Substring(0, id.IndexOf("#"));
            var apiUrl = "https://javstream.top/api/source/" + id;
            var json = await this.httpClientEx.GetJsonByPostAsync<JavavStreamDetailResult>(apiUrl, new Dictionary<string, string>
            {
                { "r","" },
                {"d","javstream.top" }
            });
            var videos = json.Data.Select(e => new JavVideo
            {
                Url = e.File,
                Type = VideoType.mp4,
                VideoQuality = VideoQualityParser.Parse(e.Label)
            }).ToList();
            return new List<IMedia>{new SimpleMedia
            {
                Videos = videos
            } };
        }
    }

    /// <summary>
    /// Defines the <see cref="JavavStreamDetailResult" />.
    /// </summary>
    public class JavavStreamDetailResult
    {
        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        public List<JavavStreamDetailResultData> Data { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="JavavStreamDetailResultData" />.
    /// </summary>
    public class JavavStreamDetailResultData
    {
        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the File.
        /// </summary>
        public string File { get; set; }
    }
}
