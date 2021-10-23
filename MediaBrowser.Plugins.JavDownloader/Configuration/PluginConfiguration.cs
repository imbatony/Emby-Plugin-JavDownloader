// -----------------------------------------------------------------------
// <copyright file="PluginConfiguration.cs" author="imbatony">
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using MediaBrowser.Model.Plugins;
    using MediaBrowser.Plugins.JavDownloader.Extensions;

    /// <summary>
    /// Defines the <see cref="PluginConfiguration" />.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether EnableExtractionDuringLibraryScan.
        /// </summary>
        public bool EnableExtractionDuringLibraryScan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableLocalMediaFolderSaving.
        /// </summary>
        public bool EnableLocalMediaFolderSaving { get; set; }

        /// <summary>
        /// Gets or sets the ConfigurationVersion
        /// 最后修改时间...
        /// </summary>
        public long ConfigurationVersion { get; set; } = DateTime.Now.Ticks;

        /// <summary>
        /// Gets the Version
        /// 版本信息..
        /// </summary>
        public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Gets or sets the ProxyType
        /// 代理服务器类型..
        /// </summary>
        public int ProxyType { get; set; } = (int)ProxyTypeEnum.None;

        /// <summary>
        /// Gets a value indicating whether EnableJsProxy
        /// 启用代理..
        /// </summary>
        public bool EnableJsProxy => ProxyType == (int)ProxyTypeEnum.JsProxy && JsProxy.IsWebUrl();

        /// <summary>
        /// Gets or sets the JsProxy
        /// JsProxy 代理地址..
        /// </summary>
        public string JsProxy { get; set; } = "https://j.javscraper.workers.dev/";

        /// <summary>
        /// Defines the default_jsProxyBypass.
        /// </summary>
        private const string default_jsProxyBypass = "netcdn.";

        /// <summary>
        /// Defines the _jsProxyBypass.
        /// </summary>
        private List<string> _jsProxyBypass;

        /// <summary>
        /// Gets or sets the JsProxyBypass
        /// 不走代理的域名..
        /// </summary>
        public string JsProxyBypass
        {
            get => _jsProxyBypass?.Any() != true ? default_jsProxyBypass : string.Join(",", _jsProxyBypass);
            set
            {
                _jsProxyBypass = value?.Split(" ,;，；".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).Select(o => o.Trim())
                    .Distinct().ToList() ?? new List<string>();
            }
        }

        /// <summary>
        /// 是否不走代理.
        /// </summary>
        /// <param name="host">The host<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsBypassed(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return false;
            if (_jsProxyBypass == null)
                JsProxyBypass = default_jsProxyBypass;

            return _jsProxyBypass?.Any(v => host.IndexOf(v, StringComparison.OrdinalIgnoreCase) >= 0) == true;
        }

        /// <summary>
        /// Gets or sets the ProxyHost
        /// 代理服务器：主机..
        /// </summary>
        public string ProxyHost { get; set; } = "127.0.0.1";

        /// <summary>
        /// Gets or sets the ProxyPort
        /// 代理服务器：端口..
        /// </summary>
        public int ProxyPort { get; set; } = 7890;

        /// <summary>
        /// Gets or sets the ProxyUserName
        /// 代理服务器：用户名..
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// Gets or sets the ProxyPassword
        /// 代理服务器：密码..
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether EnableX_FORWARDED_FOR
        /// 启用 X-FORWARDED-FOR 配置..
        /// </summary>
        public bool EnableX_FORWARDED_FOR { get; set; } = true;

        /// <summary>
        /// Gets or sets the X_FORWARDED_FOR
        /// X-FORWARDED-FOR IP地址..
        /// </summary>
        public string X_FORWARDED_FOR { get; set; } = "17.172.224.99";


        public string DownloadPath { get; set; } = Environment.GetEnvironmentVariable("JavDownloaderDownloadPath") ?? Path.GetTempPath();
}

    /// <summary>
    /// 代理类型
    /// </summary>
    public enum ProxyTypeEnum
    {
        /// <summary>
        /// Defines the None.
        /// </summary>
        None = -1,
        /// <summary>
        /// Defines the JsProxy.
        /// </summary>
        JsProxy,
        /// <summary>
        /// Defines the HTTP.
        /// </summary>
        HTTP,
        /// <summary>
        /// Defines the HTTPS.
        /// </summary>
        HTTPS,
        /// <summary>
        /// Defines the Socks5.
        /// </summary>
        Socks5
    }
}
