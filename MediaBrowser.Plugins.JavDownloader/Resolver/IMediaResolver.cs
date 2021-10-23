// -----------------------------------------------------------------------
// <copyright file="IMediaResolver.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="IMediaResolver" />.
    /// </summary>
    public interface IMediaResolver
    {
        /// <summary>
        /// The GetMedias.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="List{IMedia}"/>.</returns>
        Task<List<IMedia>> GetMedias(string url);
    }
}
