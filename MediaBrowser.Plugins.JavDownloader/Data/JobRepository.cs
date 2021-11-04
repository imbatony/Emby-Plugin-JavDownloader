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
    using System.Threading.Tasks;
    using LiteDB;
    using MediaBrowser.Plugins.JavDownloader.Job;
    using MediaBrowser.Plugins.JavDownloader.Utils;

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

        /// <summary>
        /// The Count.
        /// </summary>
        /// <param name="conditionPredicates">The conditionPredicates.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public int Count(params (bool condition, Expression<Func<JobModel, bool>> expression)[] conditionPredicates)
        {
            var exp = this.MergeExp(conditionPredicates);
            return this.downloadJobs.Count(exp);
        }

        /// <summary>
        /// The MergeExp.
        /// </summary>
        /// <param name="conditionPredicates">The conditionPredicates<see cref="(bool condition, Expression{Func{JobModel, bool}} expression)[]"/>.</param>
        /// <returns>The <see cref="Expression{Func{JobModel, bool}}"/>.</returns>
        private Expression<Func<JobModel, bool>> MergeExp(params (bool condition, Expression<Func<JobModel, bool>> expression)[] conditionPredicates)
        {
            Expression<Func<JobModel, bool>> exp = (e) => true;
            if (conditionPredicates != null && conditionPredicates.Any())
            {
                foreach ((bool condition, Expression<Func<JobModel, bool>> expression) c in conditionPredicates)
                {
                    if (c.condition)
                    {
                        exp = exp.And(c.expression);
                    }
                }
            }
            return exp;
        }

        /// <summary>
        /// The BuildPageList.
        /// </summary>
        /// <param name="totalCount">The totalCount<see cref="int"/>.</param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="items">The items<see cref="IEnumerable{TEntity}"/>.</param>
        /// <returns>The <see cref="PagedList{TEntity}"/>.</returns>
        private PagedList<T> BuildPageList<T>(int totalCount, int pageIndex, int pageSize, IEnumerable<T> items) where T : new()
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PagedList<T>
            {
                TotalCount = totalCount,
                Items = items,
                TotalPages = totalPages,
                HasNextPages = pageIndex < totalPages,
                HasPrevPages = pageIndex > 1,
                PageIndex = 1,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// The GetPagedList.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="conditionPredicates">The predicate.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public PagedList<T> GetPagedList<T>(int pageIndex, int pageSize, params (bool condition, Expression<Func<JobModel, bool>> expression)[] conditionPredicates) where T : IJob, new()
        {
            var totalCount = this.Count(conditionPredicates);
            var exp = this.MergeExp(conditionPredicates);
            var jobs = this.GetJobs<T>(exp, (pageIndex - 1) * pageSize, pageSize);
            return BuildPageList(totalCount, pageIndex, pageSize, jobs);
        }

        /// <summary>
        /// The GetPagedList.
        /// </summary>
        /// <param name="pageIndex">The pageIndex<see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="conditionPredicates">The predicate.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public PagedList<JobModel> GetPagedList(int pageIndex, int pageSize, params (bool condition, Expression<Func<JobModel, bool>> expression)[] conditionPredicates)
        {
            var totalCount = this.Count(conditionPredicates);
            var exp = this.MergeExp(conditionPredicates);
            var jobs = this.downloadJobs.Find(exp, (pageIndex - 1) * pageSize, pageSize);
            return BuildPageList(totalCount, pageIndex, pageSize, jobs);
        }
    }
}
