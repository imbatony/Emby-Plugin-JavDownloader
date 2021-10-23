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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IHttpClientEx" />.
    /// </summary>
    public interface IHttpClientEx
    {
        /// <summary>
        /// Gets the BaseAddress.
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUri);

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);

        /// <summary>
        /// The GetClient.
        /// </summary>
        /// <returns>The <see cref="HttpClient"/>.</returns>
        HttpClient GetClient();

        /// <summary>
        /// The GetStreamAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Stream}"/>.</returns>
        Task<Stream> GetStreamAsync(string requestUri);

        /// <summary>
        /// The GetStringAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{string}"/>.</returns>
        Task<string> GetStringAsync(string requestUri);

        /// <summary>
        /// The PostAsync.
        /// </summary>
        /// <param name="requestUri">The requestUri<see cref="string"/>.</param>
        /// <param name="content">The content<see cref="HttpContent"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="message">The message<see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }
}
