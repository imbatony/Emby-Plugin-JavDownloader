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
        /// Gets the Videos.
        /// </summary>
        public List<VideoInfo> Videos { get; set; }

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
        public static List<DownloadItem> FromMedias(List<SimpleMedia> medias, string targetPath)
        {
            return medias.Select(e =>
            {
                if (!e.Videos.Any())
                {
                    return null;
                }
                return new DownloadItem
                {
                    FolderPath = targetPath,
                    FileName = $"{e.Num}.mp4",
                    Quality = VideoQualityParser.ToString(e.Videos[0].VideoQuality),
                    Num = e.Num,
                    FileType = e.Videos[0].Type.ToString(),
                    Videos = e.Videos
                };
            }).ToList();
        }
    }
}
