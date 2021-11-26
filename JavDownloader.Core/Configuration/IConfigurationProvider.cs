// -----------------------------------------------------------------------
// <copyright file="IConfigurationProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Core.Configuration
{
    /// <summary>
    /// Defines the <see cref="IConfigurationProvider" />.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// The GetConfig.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetConfig(string key);
    }
}
