// -----------------------------------------------------------------------
// <copyright file="SuperJavProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="SuperJavProvider" />.
    /// </summary>
    public class SuperJavProvider : BaseJavProvider
    {
        /// <summary>
        /// Gets or sets the PopularResolver.
        /// </summary>
        public IListConentResolver PopularResolver { get; set; }

        /// <summary>
        /// Gets or sets the DetailResolver.
        /// </summary>
        public IMediaResolver DetailResolver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperJavProvider"/> class.
        /// </summary>
        /// <param name="base_url">The base_url<see cref="string"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        public SuperJavProvider(string base_url, ILogger log) : base(base_url, log)
        {
            this.PopularResolver = new SuperJavPopulorResolver(BaseUrl, this.client);
        }

        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="List{IMedia}"/>.</returns>
        public override List<IMedia> GetTodayPopular()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IMedia}"/>.</returns>
        public override Task<IMedia> Resolve(string url)
        {
            throw new System.NotImplementedException();
        }
    }
}
