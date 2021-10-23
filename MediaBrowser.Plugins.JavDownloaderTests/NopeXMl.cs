// -----------------------------------------------------------------------
// <copyright file="PluginTests.cs" author="imbatony">
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Tests
{
    using System;
    using System.IO;
    using MediaBrowser.Model.Serialization;
    using MediaBrowser.Plugins.JavDownloader.Configuration;

    public class NopeXMl : IXmlSerializer
    {
        private readonly PluginConfiguration configuration;

        public NopeXMl(PluginConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public object DeserializeFromBytes(Type type, byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public object DeserializeFromFile(Type type, string file)
        {
            return configuration;
        }

        public object DeserializeFromStream(Type type, Stream stream)
        {
            return configuration;
        }

        public void SerializeToFile(object obj, string file)
        {

        }

        public void SerializeToStream(object obj, Stream stream)
        {

        }
    }
}
