namespace MediaBrowser.Plugins.JavDownloader.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="PagedList{TEntity}" />.
    /// </summary>
    /// <typeparam name="TEntity">.</typeparam>
    public class PagedList<TEntity>
          where TEntity : new()
    {
        /// <summary>
        /// Gets or sets the PageIndex.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the PageSize.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the TotalCount.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the TotalPages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the Items.
        /// </summary>
        public IEnumerable<TEntity> Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HasPrevPages.
        /// </summary>
        public bool HasPrevPages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether HasNextPages.
        /// </summary>
        public bool HasNextPages { get; set; }
    }
}
