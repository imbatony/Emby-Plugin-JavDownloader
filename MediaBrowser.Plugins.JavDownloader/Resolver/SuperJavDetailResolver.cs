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
    using MediaBrowser.Plugins.JavDownloader.Utils.Emby.Plugins.JavScraper;

    /// <summary>
    /// Defines the <see cref="SuperJavDetailResolver" />.
    /// </summary>
    public class SuperJavDetailResolver : AbstractMediaResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuperJavDetailResolver"/> class.
        /// </summary>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public SuperJavDetailResolver(IHttpClientEx httpClientEx, ILogger logger) : base(httpClientEx, logger)
        {
        }

        /// <summary>
        /// The GetMedias.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public override async Task<List<IMedia>> GetMedias(string url)
        {
            var detail = await httpClientEx.GetHtmlDocumentAsync(url);
            var linkNodes = detail.DocumentNode.SelectNodes("//a[@data-link]");
            var title = detail.DocumentNode.SelectSingleNode("//div[@class='archive-title']/h1").InnerText;
            var links = linkNodes.Select(e => e.GetAttributeValue("data-link", "").DecodeBase64String()).Where(e => e.IsWebUrl()).Distinct();
            var cds = detail.DocumentNode.SelectNodes("//a[contains(@class,'btn-cd')]");
            var streamtapes = links.Where(e => e.Contains("streamtape")).ToList();
            if (cds != null && cds.Any())
            {
                var list = new List<IMedia>();
                for (int i = 0; i < streamtapes.Count; i++)
                {
                    var media = new SimpleMedia
                    {
                        Title = title,
                        Num = title.ExtractKey(),
                        Url = url,
                        Provider = "SuperJav",
                        Part = $"{i + 1}"
                    };
                    var medias = new SteamTapeResolver(httpClientEx, logger, media).GetMedias(streamtapes[i]).Result;
                    list.AddRange(medias);
                }

                return list;
            }
            else if (streamtapes.Any())
            {
                var media = new SimpleMedia
                {
                    Title = title,
                    Num = JavIdRecognizer.Parse(title),
                    Url = url,
                    Provider = "SuperJav",
                    Part = "1"
                };
                return new SteamTapeResolver(httpClientEx, logger, media).GetMedias(streamtapes[0]).Result;
            }
            else
            {
                return new List<IMedia>();
            }
        }
    }
}
