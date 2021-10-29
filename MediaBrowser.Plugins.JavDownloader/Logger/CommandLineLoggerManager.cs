namespace MediaBrowser.Plugins.JavDownloader.Logger
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediaBrowser.Model.Logging;

    /// <summary>
    /// Defines the <see cref="CommandLineLoggerManager" />.
    /// </summary>
    public class CommandLineLoggerManager : ILogManager
    {
        /// <summary>
        /// Gets or sets the LogSeverity.
        /// </summary>
        public LogSeverity LogSeverity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the ExceptionMessagePrefix.
        /// </summary>
        public string ExceptionMessagePrefix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Defines the LoggerLoaded.
        /// </summary>
        public event EventHandler LoggerLoaded;

        /// <summary>
        /// The AddConsoleOutput.
        /// </summary>
        public void AddConsoleOutput()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Flush.
        /// </summary>
        public void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetLogger.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public ILogger GetLogger(string name)
        {
            return new CommandLineLogger(Console.WriteLine);
        }

        /// <summary>
        /// The ReloadLogger.
        /// </summary>
        /// <param name="severity">The severity<see cref="LogSeverity"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task ReloadLogger(LogSeverity severity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The RemoveConsoleOutput.
        /// </summary>
        public void RemoveConsoleOutput()
        {
            throw new NotImplementedException();
        }
    }
}
