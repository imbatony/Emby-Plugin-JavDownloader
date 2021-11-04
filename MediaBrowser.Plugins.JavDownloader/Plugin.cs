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
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Plugins;
    using MediaBrowser.Model.Serialization;
    using MediaBrowser.Plugins.JavDownloader.Configuration;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Provider;

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
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public Plugin(
            IApplicationPaths applicationPaths,
            IXmlSerializer xmlSerializer,
            ILogManager logManager
            ) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
            logger = logManager.GetLogger("Plugin");
            logger?.Info($"{Name} - Loaded.");
            if (applicationPaths != null)
            {
                var db = ApplicationDbContext.Create(applicationPaths);
                this.JobRepository = new JobRepository(db);
            }
            this.logManager = logManager;
        }

        /// <summary>
        /// Gets the DB
        /// 数据库......
        /// </summary>
        public JobRepository JobRepository { get; private set; }

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
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html",
                    EnableInMainMenu =true,
                    MenuSection = "server",
                    MenuIcon = "theaters",
                    DisplayName = "Jav Downloader",
                }
            };
        }

        /// <summary>
        /// The CreateProvider.
        /// </summary>
        private void CreateProvider()
        {
            var providers = new List<IJavProvider>
            {
                new SuperJavProvider("https://supjav.com",logManager.GetLogger("superjav"))
            };
            if (Configuration.EnableYoutubeDL)
            {
                providers.Add(new YoutubeDLProvider(Configuration.YoutubeDLPath, Configuration.FFmpegPath, logManager.GetLogger("youtube-dl")));
            }
            this._provider = new CompositeJavProvider(providers);
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
        /// 名称......
        /// </summary>
        public const string NAME = "JavDownloader";

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public override string Name => NAME;

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

        /// <summary>
        /// The SetTestConf.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="PluginConfiguration"/>.</param>
        public void SetConf(PluginConfiguration configuration)
        {
            this.Configuration = configuration;
            Configuration.ConfigurationVersion = DateTime.Now.Ticks;
            CreateProvider();
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Defines the logManager.
        /// </summary>
        private readonly ILogManager logManager;

        /// <summary>
        /// Gets the javProvider.
        /// </summary>
        public CompositeJavProvider javProvider
        {
            get
            {
                if (_provider != null)
                {
                    return _provider;
                }
                else
                {
                    CreateProvider();
                    return _provider;
                }
            }
        }

        /// <summary>
        /// Defines the _provider.
        /// </summary>
        private CompositeJavProvider _provider;

        /// <summary>
        /// The SaveConfiguration.
        /// </summary>
        public override void SaveConfiguration()
        {
            Configuration.ConfigurationVersion = DateTime.Now.Ticks;
            base.SaveConfiguration();
        }

        /// <summary>
        /// The UpdateConfiguration.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="BasePluginConfiguration"/>.</param>
        public override void UpdateConfiguration(BasePluginConfiguration configuration)
        {
            base.UpdateConfiguration(configuration);
            CreateProvider();
        }
    }
}
