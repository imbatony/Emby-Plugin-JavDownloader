// -----------------------------------------------------------------------
// <copyright file="JavStreamDetailResolverTests.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider.Tests
{
    using System.IO;
    using MediaBrowser.Common.Configuration;

    public class TestIApplicationPaths : IApplicationPaths
    {
        public string ProgramDataPath => throw new System.NotImplementedException();

        public string ProgramSystemPath => throw new System.NotImplementedException();

        public string DataPath => throw new System.NotImplementedException();

        public string VirtualDataPath => throw new System.NotImplementedException();

        public string PluginsPath => throw new System.NotImplementedException();

        public string PluginConfigurationsPath => Path.GetTempPath();

        public string TempUpdatePath => throw new System.NotImplementedException();

        public string LogDirectoryPath => throw new System.NotImplementedException();

        public string ConfigurationDirectoryPath => Path.GetTempPath();

        public string SystemConfigurationFilePath => throw new System.NotImplementedException();

        public string CachePath => throw new System.NotImplementedException();

        public string TempDirectory => throw new System.NotImplementedException();

        public System.ReadOnlySpan<char> GetCachePath()
        {
            throw new System.NotImplementedException();
        }

        public System.ReadOnlySpan<char> GetImageCachePath()
        {
            throw new System.NotImplementedException();
        }
    }
}
