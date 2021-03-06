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
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            mock.Setup(e => e.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(Helper.GetSampleContent("steamtape.html")) });
            this.resolver = new SuperJavDetailResolver(mock.Object, new Mock<ILogger>().Object);
            var result = this.resolver.GetMedias("https://supjav.com/zh/118580.html").Result;
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest2()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            mock.Setup(e => e.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(Helper.GetSampleContent("steamtape2.html")) });
            this.resolver = new SuperJavDetailResolver(mock.Object, new Mock<ILogger>().Object);
            var result = this.resolver.GetMedias("https://supjav.com/zh/118580.html").Result;
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest3()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            mock.Setup(e => e.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(Helper.GetSampleContent("steamtape3.html")) });
            this.resolver = new SuperJavDetailResolver(mock.Object, new Mock<ILogger>().Object);
            var result = this.resolver.GetMedias("https://supjav.com/zh/118580.html").Result;
            Assert.AreEqual(2, result.Count);
        }
    }
}
