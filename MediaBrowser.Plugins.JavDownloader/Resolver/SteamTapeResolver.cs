namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
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
            var videoLinkNode = detail.DocumentNode.SelectSingleNode("//div[@id='robotlink']");
            var videoLink = videoLinkNode.InnerHtml.Replace("/streamtape.com/get_video?", "");
            videoLink = videoLink.Substring(0, videoLink.IndexOf("token"));
            var scirpt = detail.Text.Substring(detail.Text.LastIndexOf(videoLink),100);
            var realtoken = scirpt.Substring(0, scirpt.IndexOf("').substring"));
            videoLink = $"http://streamtape.com/get_video?{realtoken}&stream=1";
            simpleMedia.Videos = new List<JavVideo>
            {
                new JavVideo
                {
                    Type = VideoType.mp4,
                    Url = videoLink,
                    VideoQuality = VideoQuality.p720
                }
            };

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
