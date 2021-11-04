namespace MediaBrowser.Plugins.JavDownloader.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;
    using MediaBrowser.Model.Services;
    using MediaBrowser.Plugins.JavDownloader.Data;
    using MediaBrowser.Plugins.JavDownloader.Job;

    /// <summary>
    /// Defines the <see cref="ResolveReqeust" />.
    /// </summary>
    [Route("/emby/Plugins/JavDownloader/Jobs", "GET")]
    public class JobReqeust : IReturn<PagedList<JobModel>>
    {
        /// <summary>
        /// Gets or sets the Page.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the PageSize.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public int? Status { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="JobService" />.
    /// </summary>
    public class JobService : IService, IRequiresRequest
    {
        /// <summary>
        /// Gets or sets the Request.
        /// </summary>
        public IRequest Request { get; set; }

        /// <summary>
        /// Defines the jobs.
        /// </summary>
        private readonly JobRepository jobs;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobService"/> class.
        /// </summary>
        /// <param name="logManager">The logManager<see cref="ILogManager"/>.</param>
        public JobService(ILogManager logManager)
        {
            this.logger = logManager.GetLogger("JobService");
            this.jobs = Plugin.Instance.JobRepository;
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="request">The request<see cref="ResolveReqeust"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public PagedList<JobModel> Get(JobReqeust request)
        {
            return DoGet(request.Page, request.PageSize, request.Type, request.Status);
        }

        /// <summary>
        /// The DoGet.
        /// </summary>
        /// <param name="page">The page<see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize<see cref="int"/>.</param>
        /// <param name="type">The type<see cref="string"/>.</param>
        /// <param name="status">The status<see cref="int?"/>.</param>
        /// <returns>The <see cref="Task{List{IJob}}"/>.</returns>
        public PagedList<JobModel> DoGet(int page, int pageSize, string type, int? status)
        {
            return this.jobs.GetPagedList(page, pageSize,
                 (!string.IsNullOrEmpty(type), j => j.Type == type),
                 (status.HasValue, j => j.Status == status.Value)
             );
        }
    }
}
