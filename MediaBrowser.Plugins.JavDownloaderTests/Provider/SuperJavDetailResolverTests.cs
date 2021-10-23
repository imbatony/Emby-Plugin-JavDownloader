// -----------------------------------------------------------------------
// <copyright file="SuperJavDetailResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Http;
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
            mock.SetUpMock("", "superjavdetail.html");
            this.resolver = new SuperJavDetailResolver(mock.Object, new Mock<ILogger>().Object);
        }

        /// <summary>
        /// The GetMediasTest.
        /// </summary>
        [TestMethod()]
        public void GetMediasTest()
        {
            var result = this.resolver.GetMedias("").Result;
        }
    }
}
