namespace MediaBrowser.Plugins.JavDownloader.Provider
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="CompositeJavProvider" />.
    /// </summary>
    public class CompositeJavProvider : IJavProvider
    {
        /// <summary>
        /// Defines the providers.
        /// </summary>
        private readonly List<IJavProvider> providers;

        /// <summary>
        /// Defines the equlity.
        /// </summary>
        private readonly MediaEqualityComparer equlity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeJavProvider"/> class.
        /// </summary>
        /// <param name="providers">The providers<see cref="List{IJavProvider}"/>.</param>
        public CompositeJavProvider(List<IJavProvider> providers)
        {
            this.providers = providers;
            this.equlity = new MediaEqualityComparer();
        }

        /// <summary>
        /// Gets the Type.
        /// </summary>
        public string Type => "Composite";

        /// <summary>
        /// The GetTodayPopular.
        /// </summary>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public Task<List<IMedia>> GetTodayPopular()
        {
            return Task.FromResult(this.providers.SelectMany(p => p.GetTodayPopular().Result).Distinct(equlity).ToList());
        }

        /// <summary>
        /// The Match.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Match(string url)
        {
            return this.providers.Where(p => p.Match(url)).Any();
        }

        /// <summary>
        /// The Resolve.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{List{IMedia}}"/>.</returns>
        public Task<List<IMedia>> Resolve(string url)
        {
            return Task.FromResult(this.providers.Where(p => p.Match(url)).SelectMany(p => p.Resolve(url).Result).Distinct(equlity).ToList());
        }
    }
}
