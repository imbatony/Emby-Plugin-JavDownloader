// -----------------------------------------------------------------------
// <copyright file="HttpClientEx.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Core.Http
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DefaultHttpClientEx" />.
    /// </summary>
    public class DefaultHttpClientEx : IHttpClientEx
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private HttpClient client = null;

        /// <summary>
        /// The GetClient.
        /// </summary>
        /// <returns>The <see cref="HttpClient"/>.</returns>
        public virtual HttpClient GetClient()
        {
            if (client == null)
            {
                client = new HttpClient();
            }
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
        /// The SendAsync.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message) => GetClient().SendAsync(message);

        /// <summary>
        /// Gets the BaseAddress.
        /// </summary>
        public Uri BaseAddress => GetClient().BaseAddress;
    }
}
