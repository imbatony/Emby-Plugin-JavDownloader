// -----------------------------------------------------------------------
// <copyright file="SuperJavDetailResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using System.Net.Http;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Resolver;
    using MediaBrowser.Plugins.JavDownloaderTests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Defines the <see cref="SuperJavDetailResolverTests" />.
    /// </summary>
    [TestClass()]
    public class SuperJavDetailResolverTests
    {
        /// <summary>
        /// Defines the resolver.
        /// </summary>
        private SuperJavDetailResolver resolver;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            mock.Setup(e => e.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(Helper.GetSampleContent("steamtape.html")) });
            this.resolver = new SuperJavDetailResolver(mock.Object, new Mock<ILogger>().Object);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest()
        {
            var result = this.resolver.GetMedias("https://supjav.com/zh/118580.html").Result;
        }
    }
}
