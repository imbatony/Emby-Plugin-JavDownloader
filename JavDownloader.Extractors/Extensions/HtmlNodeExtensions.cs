// -----------------------------------------------------------------------
// <copyright file="HtmlNodeExtensions.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Extractors.Extensions
{
    using HtmlAgilityPack;

    /// <summary>
    /// Defines the <see cref="HtmlNodeExtensions" />.
    /// </summary>
    public static class HtmlNodeExtensions
    {
        /// <summary>
        /// The GetMetaContent.
        /// </summary>
        /// <param name="node">The node<see cref="HtmlNode"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetMetaContent(this HtmlNode node, string name)
        {
            return node.SelectSingleNode($"//meta[@name='{name}']").GetAttributeValue("content", string.Empty);
        }

        /// <summary>
        /// The GetMetaContent.
        /// </summary>
        /// <param name="node">The node<see cref="HtmlNode"/>.</param>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Attr(this HtmlNode node, string name)
        {
            return node.GetAttributeValue(name, string.Empty);
        }
    }
}
