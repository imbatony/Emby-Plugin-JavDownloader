namespace MediaBrowser.Plugins.JavDownloader.Data
{
    using System;
    using LiteDB;

    /// <summary>
    /// Defines the <see cref="Job" />.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the id
        /// id...
        /// </summary>
        [BsonId]
        public ObjectId id { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// 适配器...
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Num
        /// 去掉下划线和横线的番号...
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Gets or sets the Url
        /// 链接地址...
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// 链接地址...
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the Quality.
        /// </summary>
        public string Quality { get; set; } = "720p";

        /// <summary>
        /// Gets or sets the Quality.
        /// </summary>
        public string FileType { get; set; } = "mp4";

        /// <summary>
        /// Gets or sets the Modified
        /// 修改时间...
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the Created
        /// 创建时间...
        /// </summary>
        public DateTime Created { get; set; }
    }
}
