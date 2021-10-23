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
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Http;

    /// <summary>
    /// Defines the <see cref="SuperJavPopulorResolver" />.
    /// </summary>
    public class SuperJavPopulorResolver : IListConentResolver
    {
        /// <summary>
        /// Defines the baseUrl.
        /// </summary>
        private readonly string baseUrl;

        /// <summary>
        /// Defines the httpClientEx.
        /// </summary>
        private readonly IHttpClientEx httpClientEx;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperJavPopulorResolver"/> class.
        /// </summary>
        /// <param name="baseUrl">The baseUrl<see cref="string"/>.</param>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        public SuperJavPopulorResolver(string baseUrl, IHttpClientEx httpClientEx)
        {
            this.baseUrl = baseUrl;
            this.httpClientEx = httpClientEx;
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <returns>The <see cref="Task{List{string}}"/>.</returns>
        public async Task<List<string>> Resolve()
        {
            var doc = await httpClientEx.GetHtmlDocumentAsync(baseUrl+ "/zh/popular");
            var popular = doc.DocumentNode.SelectNodes("//div[@class='post']/a");
            return popular.Select(e => e.GetAttributeValue("href", string.Empty)).Where(e => e.IsWebUrl()).Distinct().ToList();
        }
    }
}
