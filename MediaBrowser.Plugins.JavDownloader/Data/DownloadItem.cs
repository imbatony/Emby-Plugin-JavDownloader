namespace MediaBrowser.Plugins.JavDownloader.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using MediaBrowser.Plugins.JavDownloader.Media;

    /// <summary>
    /// Defines the <see cref="DownloadItem" />.
    /// </summary>
    public class DownloadItem
    {
        public DownloadItem()
        {
            Extras = new Dictionary<string, string>();
        }
        /// <summary>
        /// Gets or sets the FolderPath.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Num.
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Gets or sets the Quality.
        /// </summary>
        public string Quality { get; set; } = "720p";

        /// <summary>
        /// Gets or sets the Quality.
        /// </summary>
        public string FileType { get; set; } = "mp4";

        /// <summary>
        /// Gets the Extras.
        /// </summary>
        public Dictionary<string, string> Extras { get; set; }
        /// <summary>
        /// The FromMedias.
        /// </summary>
        /// <param name="medias">The medias<see cref="List{IMedia}"/>.</param>
        /// <param name="targetPath">The targetPath<see cref="string"/>.</param>
        /// <returns>The <see cref="List{DownloadItem}"/>.</returns>
        public static List<DownloadItem> FromMedias(List<IMedia> medias, string targetPath)
        {
            return medias.SelectMany(m => m.Videos.Select(v => new DownloadItem
            {
                Url = v.Url,
                FileName = $"{m.Num}-{m.Part}-{VideoQualityParser.ToString(v.VideoQuality)}.mp4",
                Num = $"{m.Num}-{m.Part}",
                FolderPath = targetPath,
                Quality= VideoQualityParser.ToString(v.VideoQuality),
                FileType = v.Type.ToString()
            })).ToList();
        }
    }
}
