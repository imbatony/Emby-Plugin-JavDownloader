namespace MediaBrowser.Plugins.JavDownloader.Data
{
    using System.IO;
    using LiteDB;
    using MediaBrowser.Common.Configuration;

    /// <summary>
    /// 数据库访问实体.
    /// </summary>
    public class ApplicationDbContext : LiteDatabase
    {
        /// <summary>
        /// Gets the Jobs
        /// 任务列表.
        /// </summary>
        public ILiteCollection<Job> Jobs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">.</param>
        public ApplicationDbContext(string connectionString)
            : base(connectionString)
        {
            Jobs = GetCollection<Job>("Jobs");
            Jobs.EnsureIndex(o => o.Num);
        }

        /// <summary>
        /// 创建数据库实体.
        /// </summary>
        /// <param name="applicationPaths">.</param>
        /// <returns>.</returns>
        public static ApplicationDbContext Create(IApplicationPaths applicationPaths)
        {
            try
            {
                var path = Path.Combine(applicationPaths.DataPath, "JavDownloader.db");
                return new ApplicationDbContext(path);
            }
            catch { }

            return default;
        }
    }
}
