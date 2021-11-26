// -----------------------------------------------------------------------
// <copyright file="HttpClientEx.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Http
{
    using global::JavDownloader.Core.Http;
    using System;
    using System.Net.Http;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// HttpClient.
    /// </summary>
    public class ProxyHttpClientEx : DefaultHttpClientEx
    {
        /// <summary>
        /// 客户端初始话方法..
        /// </summary>
        private readonly Action<HttpClient> ac;

        /// <summary>
        /// 当前客户端..
        /// </summary>
        private HttpClient client = null;

        /// <summary>
        /// 配置版本号..
        /// </summary>
        private long version = -1;

        /// <summary>
        /// 上一个客户端..
        /// </summary>
        private HttpClient client_old = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyHttpClientEx"/> class.
        /// </summary>
        /// <param name="ac">The ac<see cref="Action{HttpClient}"/>.</param>
        public ProxyHttpClientEx(Action<HttpClient> ac = null)
        {
            this.ac = ac;
        }

        /// <summary>
        /// 获取一个 HttpClient.
        /// </summary>
        /// <returns>.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override HttpClient GetClient()
        {
            if (client != null && version == Plugin.Instance.Configuration.ConfigurationVersion)
                return client;

            if (client_old != null)
            {
                client_old.Dispose();
                client_old = null;
            }
            client_old = client;

            var handler = new ProxyHttpClientHandler(false);
            client = new HttpClient(handler, true);
            ac?.Invoke(client);

            return client;
        }     
    }
}
