// -----------------------------------------------------------------------
// <copyright file="JavStreamDetailResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using System.Net.Http;
    using MediaBrowser.Plugins.JavDownloader.Http;
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
            this.resolver = new JavStreamDetailResolver(mock.Object);
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
            Assert.AreEqual(3, result[0].Videos);
        }
    }
}
