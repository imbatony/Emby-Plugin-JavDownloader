// -----------------------------------------------------------------------
// <copyright file="Helper.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors.Tests
{
    using JavDownloader.Core.Http;
    using Moq;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Helper" />.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// The GetSample.
        /// </summary>
        /// <param name="sampleFileName">The sampleFileName<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetSampleContent(string sampleFileName)
        {
            return File.ReadAllText(Path.Combine("Sample", sampleFileName));
        }

        /// <summary>
        /// The SetUpClient.
        /// </summary>
        /// <param name="mock">The mock<see cref="Mock{IHttpClientEx}"/>.</param>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="sampleFileName">The sampleFileName<see cref="string"/>.</param>
        /// <returns>The <see cref="Mock{IHttpClientEx}"/>.</returns>
        public static Mock<IHttpClientEx> SetUpMock(this Mock<IHttpClientEx> mock, string url, string sampleFileName)
        {
            var content = GetSampleContent(sampleFileName);
            mock.Setup(e => e.GetStringAsync(It.Is<string>((e) => e == url)))
                .ReturnsAsync(content);
            return mock;
        }
    }
}
