// -----------------------------------------------------------------------
// <copyright file="PluginTests.cs" author="imbatony">
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Tests
{
    using MediaBrowser.Plugins.JavDownloader.Logger;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines the <see cref="PluginTests" />.
    /// </summary>
    [TestClass()]
    public class PluginTests
    {
        /// <summary>
        /// The PluginTest.
        /// </summary>
        [TestMethod()]
        public void PluginTest()
        {
            var plugin = new Plugin(null, null , new CommandLineLoggerManager());
            Assert.IsNotNull(plugin.GetThumbImage());
        }

        /// <summary>
        /// The GetPagesTest.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public void GetPagesTest()
        {
            var plugin = new Plugin(null, null, new CommandLineLoggerManager());
            Assert.IsNotNull(plugin.GetPluginInfo());
        }

        /// <summary>
        /// The GetThumbImageTest.
        /// </summary>
        [TestMethod()]
        public void GetThumbImageTest()
        {
            var plugin = new Plugin(null, null, new CommandLineLoggerManager());
            Assert.IsNotNull(plugin.GetThumbImage());
        }
    }
}
