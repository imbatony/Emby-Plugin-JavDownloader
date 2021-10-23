// -----------------------------------------------------------------------
// <copyright file="IListConentResolver.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Resolver
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IListConentResolver" />.
    /// </summary>
    public interface IListConentResolver
    {
        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <returns>The <see cref="Task{List{string}}"/>.</returns>
        Task<List<string>> Resolve();
    }
}
