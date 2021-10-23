// -----------------------------------------------------------------------
// <copyright file="IJavProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="IJavProvider" />.
    /// </summary>
    public interface IJavProvider
    {
        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="List{IMedia}"/>.</returns>
        Task<List<IMedia>> GetTodayPopular();

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IMedia}"/>.</returns>
        Task<IMedia> Resolve(string url);

        /// <summary>
        /// The Match.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool Match(string url);

        /// <summary>
        /// Gets the Type.
        /// </summary>
        string Type { get; }
    }
}
