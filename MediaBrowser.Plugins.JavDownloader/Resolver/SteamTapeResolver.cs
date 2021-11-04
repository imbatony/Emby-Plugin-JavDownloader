namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using HtmlAgilityPack;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="SteamTapeResolver" />.
    /// </summary>
    internal class SteamTapeResolver : AbstractMediaResolver
    {
        /// <summary>
        /// Defines the simpleMedia.
        /// </summary>
        private readonly SimpleMedia simpleMedia;

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamTapeResolver"/> class.
        /// </summary>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger"/>.</param>
        /// <param name="simpleMedia">The simpleMedia<see cref="SimpleMedia"/>.</param>
        public SteamTapeResolver(IHttpClientEx httpClientEx, ILogger logger, SimpleMedia simpleMedia) : base(httpClientEx, logger)
        {
            this.simpleMedia = simpleMedia;
        }

        /// <summary>
        /// The GetMedias.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public override async Task<List<IMedia>> GetMedias(string url)
        {
            var detail = await this.GetHtmlDocument(url);
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
                simpleMedia.Videos = new List<VideoInfo>
            {
                new VideoInfo
                {
                    Type = VideoType.mp4,
                    Url = videoLink,
                    VideoQuality = VideoQuality.p720
                }
            };
            }
            catch (Exception e)
            {
                this.logger?.ErrorException("resolve failed", e);
                this.logger?.Debug(detail.Text);
            }

            return new List<IMedia>
            {
                simpleMedia
            };
        }

        /// <summary>
        /// The GetHtmlDocument.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{HtmlDocument}"/>.</returns>
        public Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Add("referer", simpleMedia.Url);
            return this.httpClientEx.GetHtmlDocumentByReqAsync(req, this.logger);
        }
    }
}
