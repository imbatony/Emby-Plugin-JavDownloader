// -----------------------------------------------------------------------
// <copyright file="IJob.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MediaBrowser.Plugins.JavDownloader.Job
{
    using System;
    using System.Collections.Generic;
    using LiteDB;
    using MediaBrowser.Plugins.JavDownloader.Data;

    /// <summary>
    /// Defines the <see cref="IJob" />.
    /// </summary>
    public abstract class IJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJob"/> class.
        /// </summary>
        public IJob()
        {
            this.Created = DateTime.UtcNow;
            this.Modified = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the id
        /// id..........
        /// </summary>
        [BsonId]
        public ObjectId id { get; set; }

        /// <summary>
        /// Gets the Type
        /// Gets or sets the Type...
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the Modified.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the Created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The ToModel.
        /// </summary>
        /// <returns>The <see cref="JobModel"/>.</returns>
        public virtual JobModel ToModel()
        {
            return new JobModel
            {
                id = this.id,
                Status = this.Status,
                Modified = this.Modified,
                Created = this.Created,
                Extra = this.BuildExtra(),
                Type = this.Type,
                Key = this.Key
            };
        }

        /// <summary>
        /// The FromModel.
        /// </summary>
        /// <param name="model">The model<see cref="JobModel"/>.</param>
        public virtual void FromModel(JobModel model)
        {
            this.id = model.id;
            this.Status = model.Status;
            this.Key = model.Key;
            this.Modified = model.Modified;
            this.Created = model.Created;
            FromExtra(model.Extra);
        }

        /// <summary>
        /// The BuildExtra.
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, string}"/>.</returns>
        protected abstract Dictionary<string, string> BuildExtra();

        /// <summary>
        /// The BuildExtra.
        /// </summary>
        /// <param name="extra">The extra<see cref="Dictionary{string, string}"/>.</param>
        protected abstract void FromExtra(Dictionary<string, string> extra);
    }
}
