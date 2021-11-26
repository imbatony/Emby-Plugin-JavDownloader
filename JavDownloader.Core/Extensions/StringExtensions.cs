// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Core.Extensions
{
    using System;
    using System.Collections.Generic;
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

        /// <summary>
        /// Defines the regexKey.
        /// </summary>
        private static Regex regexKey = new Regex("^(?<a>[a-z0-9]{3,5})(?<b>[-_ ]*)(?<c>0{1,2}[0-9]{3,5})$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Defines the regexKey2.
        /// </summary>
        private static Regex regexKey2 = new Regex("^[0-9][a-z]+[-_a-z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 展开全部的Key.
        /// </summary>
        /// <param name="key">.</param>
        /// <returns>.</returns>
        public static List<string> GetAllKeys(this string key)
        {
            var ls = new List<string>();

            var m = regexKey2.Match(key);
            if (m.Success)
                ls.Add(key.Substring(1));

            ls.Add(key);

            m = regexKey.Match(key);
            if (m.Success)
            {
                var a = m.Groups["a"].Value;
                var b = m.Groups["b"].Value;
                var c = m.Groups["c"].Value;
                var end = c.TrimStart('0');
                var count = c.Length - end.Length - 1;
                for (int i = 0; i <= count; i++)
                {
                    var em = i > 0 ? new string('0', i) : string.Empty;
                    ls.Add($"{a}{em}{end}");
                    ls.Add($"{a}-{em}{end}");
                    ls.Add($"{a}_{em}{end}");
                }
            }

            if (key.IndexOf('-') > 0)
                ls.Add(key.Replace("-", "_"));
            if (key.IndexOf('_') > 0)
                ls.Add(key.Replace("_", "-"));

            if (ls.Count > 1)
                ls.Add(key.Replace("-", "").Replace("_", ""));

            return ls;
        }

        /// <summary>
        /// The ExtractKey.
        /// </summary>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ExtractKey(this string key)
        {
            return key.Substring(0, key.IndexOf(" ")).ToUpper();
        }

        /// <summary>
        /// The ExtractMatch.
        /// </summary>
        /// <param name="source">The source<see cref="string"/>.</param>
        /// <param name="regex">The regex<see cref="Regex"/>.</param>
        /// <param name="groupName">The groupName<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ExtractMatch(this string source, Regex regex, string groupName)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            var match = regex.Match(source);
            return match.Success ? match.Groups[groupName].Value : string.Empty;
        }
    }
}
