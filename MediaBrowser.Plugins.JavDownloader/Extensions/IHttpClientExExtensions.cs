// -----------------------------------------------------------------------
// <copyright file="IHttpClientExExtensions.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Plugins.JavDownloader.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Defines the <see cref="IHttpClientExExtensions" />.
    /// </summary>
    public static class IHttpClientExExtensions
    {
        /// <summary>
        /// 获取 HtmlDocument.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static async Task<HtmlDocument> GetHtmlDocumentAsync(this IHttpClientEx client, string requestUri, ILogger log = default)
        {
            try
            {
                var html = await client.GetStringAsync(requestUri);
                if (string.IsNullOrWhiteSpace(html) == false)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    return doc;
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {

                    log.ErrorException($"{ex.Message}", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取 HtmlDocument，通过 Post 方法提交.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="content">The content<see cref="HttpContent"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static async Task<HtmlDocument> GetHtmlDocumentByPostAsync(this IHttpClientEx client, string requestUri, HttpContent content, ILogger log = default)
        {
            try
            {
                var resp = await client.PostAsync(requestUri, content);
                if (resp.IsSuccessStatusCode == false)
                {
                    var eee = await resp.Content.ReadAsStringAsync();
                    return null;
                }

                var html = await resp.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(html) == false)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    return doc;
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {

                    log.ErrorException($"{ex.Message}", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取 HtmlDocument，通过 Post 方法提交.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="content">The content<see cref="HttpContent"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static async Task<HtmlDocument> GetHtmlDocumentByReqAsync(this IHttpClientEx client, HttpRequestMessage message, ILogger log = default)
        {
            try
            {
                var resp = await client.SendAsync(message);
                if (resp.IsSuccessStatusCode == false)
                {
                    var eee = await resp.Content.ReadAsStringAsync();
                    return null;
                }

                var html = await resp.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(html) == false)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    return doc;
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {

                    log.ErrorException($"{ex.Message}", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// 获取 HtmlDocument，通过 Post 方法提交.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="param">The param<see cref="Dictionary{string, string}"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static Task<HtmlDocument> GetHtmlDocumentByPostAsync(this IHttpClientEx client, string requestUri, Dictionary<string, string> param, ILogger log = default)
            => GetHtmlDocumentByPostAsync(client, requestUri, new FormUrlEncodedContent(param), log);


        /// <summary>
        /// 获取 JObject，通过 Post 方法提交.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="content">The content<see cref="HttpContent"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static async Task<T> GetJsonByPostAsync<T>(this IHttpClientEx client, string requestUri, HttpContent content, ILogger log = default)
        {
            try
            {
                var resp = await client.PostAsync(requestUri, content);
                if (resp.IsSuccessStatusCode == false)
                {
                    var eee = await resp.Content.ReadAsStringAsync();
                    return default;
                }

                var json = await resp.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json) == false)
                {
                    T doc = JsonConvert.DeserializeObject<T>(json);
                    return doc;
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.ErrorException($"{ex.Message}", ex);
                }
            }

            return default;
        }

        /// <summary>
        /// 获取 JObject，通过 Post 方法提交.
        /// </summary>
        /// <param name="client">The client<see cref="IHttpClientEx"/>.</param>
        /// <param name="requestUri">.</param>
        /// <param name="param">The param<see cref="Dictionary{string, string}"/>.</param>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        /// <returns>.</returns>
        public static Task<T> GetJsonByPostAsync<T>(this IHttpClientEx client, string requestUri, Dictionary<string, string> param, ILogger log = default)
            => GetJsonByPostAsync<T>(client, requestUri, new FormUrlEncodedContent(param), log);
    }
}
