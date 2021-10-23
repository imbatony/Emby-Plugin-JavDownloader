// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" author="imbatony">
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using MediaBrowser.Common.Configuration;
    using MediaBrowser.Common.Plugins;
    using MediaBrowser.Model.Drawing;
    using MediaBrowser.Model.Plugins;
    using MediaBrowser.Model.Serialization;
    using MediaBrowser.Plugins.JavDownloader.Configuration;

    /// <summary>
    /// Defines the <see cref="Plugin" />.
    /// </summary>
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IHasThumbImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">The applicationPaths<see cref="IApplicationPaths"/>.</param>
        /// <param name="xmlSerializer">The xmlSerializer<see cref="IXmlSerializer"/>.</param>
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// The GetPages.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{PluginPageInfo}"/>.</returns>
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "JavDownloader",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html"
                }
            };
        }

        /// <summary>
        /// Defines the _id.
        /// </summary>
        private Guid _id = new Guid("0B58AD46-DEE3-495D-B8C4-AA0A0CD2A83F");

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public override Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => "JavDownloader";

        /// <summary>
        /// Gets the Description.
        /// </summary>
        public override string Description => "Download Latest Popular Jav Files from internet";

        /// <summary>
        /// The GetThumbImage.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream($"{type.Namespace}.plugin.png");
        }

        /// <summary>
        /// Gets the ThumbImageFormat.
        /// </summary>
        public ImageFormat ThumbImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }

        public void SetTestConf(PluginConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static Plugin Instance { get; private set; }
    }
}
