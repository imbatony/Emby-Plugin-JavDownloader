// -----------------------------------------------------------------------
// <copyright file="PluginConfiguration.cs" author="imbatony">
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Configuration
{
    using MediaBrowser.Model.Plugins;

    /// <summary>
    /// Defines the <see cref="PluginConfiguration" />.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether EnableExtractionDuringLibraryScan.
        /// </summary>
        public bool EnableExtractionDuringLibraryScan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableLocalMediaFolderSaving.
        /// </summary>
        public bool EnableLocalMediaFolderSaving { get; set; }
    }
}
