// -----------------------------------------------------------------------
// <copyright file="HttpClientEx.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Http
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// HttpClient.
    /// </summary>
    public class HttpClientEx : IHttpClientEx
    {
        /// <summary>
        /// 客户端初始话方法.
        /// </summary>
        private readonly Action<HttpClient> ac;

        /// <summary>
        /// 当前客户端.
        /// </summary>
        private HttpClient client = null;

        /// <summary>
        /// 配置版本号.
        /// </summary>
        private long version = -1;

        /// <summary>
        /// 上一个客户端.
        /// </summary>
        private HttpClient client_old = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientEx"/> class.
        /// </summary>
        /// <param name="ac">The ac<see cref="Action{HttpClient}"/>.</param>
        public HttpClientEx(Action<HttpClient> ac = null)
        {
            this.ac = ac;
        }

        /// <summary>
        /// 获取一个 HttpClient.
        /// </summary>
        /// <returns>.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public HttpClient GetClient()
        {
            if (client != null && version == Plugin.Instance.Configuration.ConfigurationVersion)
                return client;

            if (client_old != null)
            {
                client_old.Dispose();
                client_old = null;
            }
            client_old = client;

            var handler = new ProxyHttpClientHandler();
            client = new HttpClient(handler, true);
            ac?.Invoke(client);

            return client;
        }

        /// <summary>
        /// The GetStringAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        public Task<string> GetStringAsync(string requestUri)
            => GetClient().GetStringAsync(requestUri);

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        public Task<HttpResponseMessage> GetAsync(string requestUri)
            => GetClient().GetAsync(requestUri);

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
            => GetClient().GetAsync(requestUri, cancellationToken);

        /// <summary>
        /// The GetStreamAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        public Task<Stream> GetStreamAsync(string requestUri)
            => GetClient().GetStreamAsync(requestUri);

        /// <summary>
        /// The PostAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="HttpContent"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
            => GetClient().PostAsync(requestUri, content);

        /// <summary>
        /// Gets the BaseAddress.
        /// </summary>
        public Uri BaseAddress => GetClient().BaseAddress;
    }
}

