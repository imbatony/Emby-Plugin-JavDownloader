// -----------------------------------------------------------------------
// <copyright file="SuperJavExtractor.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors
{
    using JavDownloader.Core.Configuration;
    using JavDownloader.Core.Extensions;
    using JavDownloader.Core.Extractor;
    using JavDownloader.Core.Http;
    using JavDownloader.Core.Logger;
    using JavDownloader.Extractors.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using YoutubeDLSharp.Metadata;

    /// <summary>
    /// Defines the <see cref="SuperJavExtractor" />.
    /// </summary>
    public class SuperJavExtractor : InfoExtractor
    {
        /// <summary>
        /// Defines the idRegex.
        /// </summary>
        private readonly Regex idRegex;

        /// <summary>
        /// Defines the thumbRegex.
        /// </summary>
        private readonly Regex thumbRegex;

        /// <summary>
        /// Defines the viewRegex.
        /// </summary>
        private readonly Regex viewRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperJavExtractor"/> class.
        /// </summary>
        /// <param name="configProvider">The configProvider<see cref="IConfigurationProvider"/>.</param>
        /// <param name="clientEx">The clientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        public SuperJavExtractor(IConfigurationProvider configProvider, IHttpClientEx clientEx, ILogger logger) : base(configProvider, clientEx, logger)
        {
            this.idRegex = new Regex(@"(?<id>\d*)\.html", RegexOptions.IgnoreCase);
            this.thumbRegex = new Regex(@"url\((?<url>.*)\);", RegexOptions.IgnoreCase);
            this.viewRegex = new Regex(@"(?<views>\d*)\sViews", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// The Extractor.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="VideoData"/>.</returns>
        public override async Task<VideoData> Extractor(string url)
        {
            var detail = await this.clientEx.GetHtmlDocumentAsync(url);
            var doc = detail.DocumentNode;
            var linkNodes = doc.SelectNodes("//a[@data-link]");
            var title = doc.SelectSingleNode("//div[@class='archive-title']/h1").InnerText;
            var links = linkNodes
                .Select(e => e.GetAttributeValue("data-link", "")
                .DecodeBase64String())
                .Where(e => e.IsWebUrl())
                .Distinct();
            var cds = doc.SelectNodes("//a[contains(@class,'btn-cd')]");
            var viewTxt = doc.SelectSingleNode("//span[@class='views']").InnerText.ExtractMatch(viewRegex, "views");
            var views = int.Parse(viewTxt);
            var data = new VideoData
            {
                Title = doc.SelectSingleNode("//title").InnerText,
                Tags = doc.GetMetaContent("keywords").Split(','),
                ID = url.ExtractMatch(idRegex, "id"),
                Description = doc.GetMetaContent("description"),
                AgeLimit = 18,
                Thumbnail = doc.SelectSingleNode("//div[@id='player-wrap']").Attr("style").ExtractMatch(thumbRegex, "url"),
                ViewCount = views,
                Extractor = "superjav",
                ExtractorKey = "superjav"
            };
            var streamtapesLinks = links.Where(e => e.Contains("streamtape")).ToList();
            if (streamtapesLinks.Count != 0)
            {
                await this.FillVideoAsync(data, streamtapesLinks);
            }    
            return data;
        }

        /// <summary>
        /// The Support.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool Support(string url)
        {
            return !string.IsNullOrEmpty(url) && url.StartsWith("https://supjav.com");
        }

        /// <summary>
        /// The FillVideoAsync.
        /// </summary>
        /// <param name="data">The data<see cref="VideoData"/>.</param>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task FillVideoAsync(VideoData data, IEnumerable<string> streamtapesLinks)
        {
            var formats = new List<FormatData>();
            foreach(var url in streamtapesLinks)
            {
                var detail = await this.clientEx.GetHtmlDocumentAsync(url);
                try
                {
                    var videoLinkNode = detail.DocumentNode.SelectSingleNode("//div[@id='robotlink']");
                    var videoLink = videoLinkNode.InnerHtml.Replace("/streamtape.com", "https://streamtape.com");
                    var videoUri = new Uri(videoLink);
                    var query = HttpUtility.ParseQueryString(videoUri.Query);
                    var id = query.Get("id");
                    var expires = query.Get("expires");
                    var ip = query.Get("ip");
                    var token = query.Get("token");
                    var tokenLength = token.Length;
                    var realtoken = detail.Text.Substring(detail.Text.LastIndexOf("token"), tokenLength + 6).Substring(6, tokenLength);
                    videoLink = $"http://streamtape.com/get_video?id={id}&expires={expires}&ip={ip}&token={realtoken}&stream=1";
                    formats.Add(new FormatData
                    {
                        Url = videoLink,
                        Format = "mp4"
                    });
                }
                catch (Exception e)
                {
                    this.logger?.ErrorException("resolve failed", e);
                    this.logger?.Debug(detail.Text);
                }
            }
            data.Formats = formats.ToArray();
            data.Url = formats.FirstOrDefault()?.Url;
            data.Extension = "mp4";
            data.Format = "mp4";
        }
    }
}
