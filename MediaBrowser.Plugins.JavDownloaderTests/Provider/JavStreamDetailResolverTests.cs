// -----------------------------------------------------------------------
// <copyright file="JavStreamDetailResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using System.Net.Http;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Configuration;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Logger;
    using MediaBrowser.Plugins.JavDownloader.Resolver;
    using MediaBrowser.Plugins.JavDownloader.Tests;
    using MediaBrowser.Plugins.JavDownloaderTests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Defines the <see cref="JavStreamDetailResolverTests" />.
    /// </summary>
    [TestClass()]
    public class JavStreamDetailResolverTests
    {
        /// <summary>
        /// Defines the resolver.
        /// </summary>
        private JavStreamDetailResolver resolver;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.Setup(e => e.PostAsync(It.Is<string>(e => e == "https://javstream.top/api/source/4y0q4az66m7gk3e"), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(Helper.GetSampleContent("javstreamapi.json"))
            });
            this.resolver = new JavStreamDetailResolver(mock.Object, new Mock<ILogger>().Object);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest()
        {
            var result = resolver.GetMedias("https://javstream.top/v/4y0q4az66m7gk3e#supjav.com@dvdms-433-1.mp4").Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public void GetMediasTest2()
        {
            var conf = new PluginConfiguration();
            Plugin plugin = new Plugin(new TestIApplicationPaths(), new NopeXMl(conf),new CommandLineLoggerManager());
            plugin.SetConf(conf);
            var resolver = new SuperJavDetailResolver(new HttpClientEx(), new Mock<ILogger>().Object);
            var medias = resolver.GetMedias("https://supjav.com/zh/118580.html").Result;
            Assert.IsNotNull(medias);
            Assert.AreEqual(1, medias.Count);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public void GetMediasTest3()
        {
            var conf = new PluginConfiguration();
            Plugin plugin = new Plugin(new TestIApplicationPaths(), new NopeXMl(conf), new CommandLineLoggerManager());
            plugin.SetConf(conf);
            var resolver = new SuperJavProvider("https://supjav.com", new Mock<ILogger>().Object);
            var medias = resolver.GetTodayPopular().Result;
            Assert.IsNotNull(medias);
            Assert.AreEqual(1, medias.Count);
        }
    }
}
