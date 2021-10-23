// -----------------------------------------------------------------------
// <copyright file="BaseJavProvider.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Extensions;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="BaseJavProvider" />.
    /// </summary>
    public abstract class BaseJavProvider : IJavProvider
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        protected IHttpClientEx client;

        /// <summary>
        /// Defines the log.
        /// </summary>
        protected ILogger log;

        /// <summary>
        /// Defines the locker.
        /// </summary>
        private static NamedLockerAsync locker = new NamedLockerAsync();

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public string Type
        {
            get
            {
                string type = GetType().Name;
                return type.Replace("JavProvider", "");
            }
        }

        /// <summary>
        /// Gets the DefaultBaseUrl
        /// 默认的基础URL......
        /// </summary>
        public string DefaultBaseUrl { get; }

        /// <summary>
        /// Gets or sets the BaseUrl
        /// 基础URL.....
        /// </summary>
        public string BaseUrl
        {
            get => base_url;
            set
            {
                if (value.IsWebUrl() != true)
                    return;
                if (base_url == value && client != null)
                    return;
                base_url = value;
                client = new HttpClientEx(client => client.BaseAddress = new Uri(base_url));
            }
        }

        /// <summary>
        /// 基础URL......
        /// </summary>
        private string base_url = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJavProvider"/> class.
        /// </summary>
        /// <param name="base_url">基础URL.</param>
        /// <param name="log">日志记录器.</param>
        public BaseJavProvider(string base_url, ILogger log)
        {
            this.log = log;
            DefaultBaseUrl = base_url;
            BaseUrl = base_url;
        }

        /// <summary>
        /// The SetClient.
        /// </summary>
        /// <param name="httpClientEx">The httpClientEx<see cref="IHttpClientEx"/>.</param>
        public void SetClient(IHttpClientEx httpClientEx)
        {
            this.client = httpClientEx;
        }

        /// <summary>
        /// 补充完整url.
        /// </summary>
        /// <param name="base_uri">基础url.</param>
        /// <param name="url">url或者路径.</param>
        /// <returns>.</returns>
        protected virtual string FixUrl(Uri base_uri, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            if (url.IsWebUrl())
                return url;

            try
            {
                return new Uri(base_uri, url).ToString();
            }
            catch { }

            return null;
        }

        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="List{IMedia}"/>.</returns>
        public abstract Task<List<IMedia>> GetTodayPopular();


        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IMedia}"/>.</returns>
        public abstract Task<List<IMedia>> Resolve(string url);

        /// <summary>
        /// The Match.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Match(string url)
        {
            return url.StartsWith(base_url);
        }
    }
}
