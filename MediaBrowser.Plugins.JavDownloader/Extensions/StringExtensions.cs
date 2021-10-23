// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="StringExtensions" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Defines the WebUrlExpression.
        /// </summary>
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// The IsWebUrl.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        /// <summary>
        /// The TrimStart.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <param name="trimString">The trimString<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString))
                return target;

            string result = target;
            while (result.StartsWith(trimString, StringComparison.OrdinalIgnoreCase))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        /// <summary>
        /// The TrimEnd.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <param name="trimStrings">The trimStrings<see cref="string[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string TrimEnd(this string target, params string[] trimStrings)
        {
            trimStrings = trimStrings?.Where(o => string.IsNullOrEmpty(o) == false).Distinct().ToArray();
            if (trimStrings?.Any() != true)
                return target;

            var found = false;

            do
            {
                found = false;
                foreach (var trimString in trimStrings)
                {
                    while (target.EndsWith(trimString, StringComparison.OrdinalIgnoreCase))
                    {
                        target = target.Substring(0, target.Length - trimString.Length);
                        found = true;
                    }
                }
            } while (found);
            return target;
        }

        /// <summary>
        /// The Trim.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <param name="trimString">The trimString<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Trim(this string target, string trimString)
            => target.TrimStart(trimString).TrimEnd(trimString);

        /// <summary>
        /// The DecodeBase64String.
        /// </summary>
        /// <param name="source">The source<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string DecodeBase64String(this string source)
        {
            var result = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                result = Encoding.ASCII.GetString(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }
    }
}
