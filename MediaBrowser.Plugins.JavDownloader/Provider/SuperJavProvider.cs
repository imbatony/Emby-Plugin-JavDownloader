// -----------------------------------------------------------------------
// <copyright file="SuperJavProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Media;
    using MediaBrowser.Plugins.JavDownloader.Resolver;

    /// <summary>
    /// Defines the <see cref="SuperJavProvider" />.
    /// </summary>
    public class SuperJavProvider : BaseJavProvider
    {
        /// <summary>
        /// Gets or sets the PopularResolver.
        /// </summary>
        private readonly IListConentResolver popularResolver;

        /// <summary>
        /// Gets or sets the DetailResolver.
        /// </summary>
        private readonly IMediaResolver detailResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperJavProvider"/> class.
        /// </summary>
        /// <param name="base_url">The base_url<see cref="string"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        public SuperJavProvider(string base_url, ILogger log) : base(base_url, log)
        {
            this.popularResolver = new SuperJavPopulorResolver(BaseUrl, this.client);
            this.detailResolver = new SuperJavDetailResolver(this.client, log);
        }

        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="List{IMedia}"/>.</returns>
        public override async Task<List<IMedia>> GetTodayPopular()
        {
            var list = await this.popularResolver.Resolve();
            return list.SelectMany(e => this.Resolve(e).Result).Distinct(new MediaEqualityComparer()).ToList();
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IMedia}"/>.</returns>
        public override Task<List<IMedia>> Resolve(string url)
        {
            return this.detailResolver.GetMedias(url);
        }
    }
}
