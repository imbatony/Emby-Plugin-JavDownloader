namespace MediaBrowser.Plugins.JavDownloader.Media
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="MediaEqualityComparer" />.
    /// </summary>
    public class MediaEqualityComparer : IEqualityComparer<IMedia>
    {
        /// <summary>
        /// The Equals.
        /// </summary>
        /// <param name="x">The x<see cref="IMedia"/>.</param>
        /// <param name="y">The y<see cref="IMedia"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Equals(IMedia x, IMedia y)
        {
            return x.Num == y.Num;
        }

        /// <summary>
        /// The GetHashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="IMedia"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode(IMedia obj)
        {
            return obj.Num.GetHashCode();
        }
    }
}
