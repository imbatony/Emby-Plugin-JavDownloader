// -----------------------------------------------------------------------
// <copyright file="SuperJavPopulorResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using System.Linq;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Logger;
    using MediaBrowser.Plugins.JavDownloader.Resolver;
    using MediaBrowser.Plugins.JavDownloaderTests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Defines the <see cref="SuperJavPopulorResolverTests" />.
    /// </summary>
    [TestClass()]
    public class SuperJavPopulorResolverTests
    {
        /// <summary>
        /// Defines the resolver.
        /// </summary>
        private SuperJavPopulorResolver resolver;

        /// <summary>
        /// The Init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("/zh/popular", "superjavpopular.html");
            this.resolver = new SuperJavPopulorResolver("", mock.Object);
        }

        /// <summary>
        /// The SuperJavPopulorResolverTest.
        /// </summary>
        [TestMethod()]
        public void SuperJavPopulorResolverTest()
        {
            Assert.IsNotNull(this.resolver);
        }

        /// <summary>
        /// The ResolveTest.
        /// </summary>
        [TestMethod()]
        public void ResolveTest()
        {
            new Plugin(null, null, new CommandLineLoggerManager());
            var list = this.resolver.Resolve().Result;
            Assert.IsNotNull(list);
            Assert.AreEqual(24, list.Count());
        }
    }
}
