// -----------------------------------------------------------------------
// <copyright file="SuperJavExtractorTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors.Tests
{
    using JavDownloader.Core.Configuration;
    using JavDownloader.Core.Http;
    using JavDownloader.Core.Logger;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="SuperJavExtractorTests" />.
    /// </summary>
    [TestClass()]
    public class SuperJavExtractorTests
    {
        /// <summary>
        /// The SuperJavExtractorTest.
        /// </summary>
        [TestMethod()]
        public void SuperJavExtractorTest()
        {
            var mock = new Mock<IHttpClientEx>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
        }

        /// <summary>
        /// The ExtractorTest.
        /// </summary>
        [TestMethod()]
        public async Task ExtractorTest()
        {
            var mock = new Mock<IHttpClientEx>();
            var logger = new Mock<ILogger>();
            var config = new Mock<IConfigurationProvider>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            mock.SetUpMock("https://streamtape.com/e/RwkBOVjrJDudorW/dvdms-433-1.mp4", "steamtape.html");
            mock.SetUpMock("https://streamtape.com/e/94e9Qvzr2jsZRW/dvdms-433-2.mp4", "steamtape2.html");
            var extractor = new SuperJavExtractor(config.Object, mock.Object, logger.Object);
            var data = await extractor.Extractor("https://supjav.com/zh/118580.html");
            Assert.AreEqual("118580", data.ID);
        }

        /// <summary>
        /// The SupportTest.
        /// </summary>
        [TestMethod()]
        public void SupportTest()
        {
            var mock = new Mock<IHttpClientEx>();
            var logger = new Mock<ILogger>();
            var config = new Mock<IConfigurationProvider>();
            mock.SetUpMock("https://supjav.com/zh/118580.html", "superjavdetail.html");
            var extractor = new SuperJavExtractor(config.Object, mock.Object, logger.Object);
            Assert.IsTrue(extractor.Support("https://supjav.com/zh/118580.html"));
        }
    }
}
