// -----------------------------------------------------------------------
// <copyright file="JobRepository.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using LiteDB;
    using MediaBrowser.Plugins.JavDownloader.Job;

    /// <summary>
    /// Defines the <see cref="JobRepository" />.
    /// </summary>
    public class JobRepository
    {
        /// <summary>
        /// Defines the downloadJobs.
        /// </summary>
        private readonly ILiteCollection<JobModel> downloadJobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The dbContext<see cref="ApplicationDbContext"/>.</param>
        public JobRepository(ApplicationDbContext dbContext)
        {

            this.downloadJobs = dbContext.GetCollection<JobModel>("DownloadJobs");
            this.downloadJobs.EnsureIndex(o => o.Key);
        }

        /// <summary>
        /// The IsExist.
        /// </summary>
        /// <param name="job">The job<see cref="IJob"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool IsExist(IJob job)
        {
            return this.downloadJobs.Exists(j => j.Type == job.Type && j.Key == job.Key);
        }

        /// <summary>
        /// The UpserJob.
        /// </summary>
        /// <param name="job">The job<see cref="IJob"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool UpsertJob(IJob job)
        {
            return this.downloadJobs.Upsert(job.ToModel());
        }

        /// <summary>
        /// The GetJob.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public T GetJob<T>(string key) where T : IJob, new()
        {
            var t = new T();
            var job = this.downloadJobs.FindOne(j => j.Type == t.Type && j.Key == key);
            if (job == null)
            {
                return default;
            }
            t.FromModel(job);
            return t;
        }

        /// <summary>
        /// The GetUnfinishedJobs.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="predicate">The predicate<see cref="BsonExpression"/>.</param>
        /// <param name="skip">The skip<see cref="int"/>.</param>
        /// <param name="limit">The limit<see cref="int"/>.</param>
        /// <returns>The <see cref="List{T}"/>.</returns>
        public List<T> GetJobs<T>(Expression<Func<JobModel, bool>> predicate, int skip = 0, int limit = int.MaxValue) where T : IJob, new()
        {
            var jobModels = this.downloadJobs.Find(predicate, skip, limit);
            return jobModels.Select(j =>
            {
                var t = new T();
                t.FromModel(j);
                return t;
            }).ToList();
        }
    }
}
