namespace MediaBrowser.Plugins.JavDownloader.Logger
{
    using System;
    using System.Text;
    using MediaBrowser.Model.Logging;

    /// <summary>
    /// Defines the <see cref="CommandLineLogger" />.
    /// </summary>
    public class CommandLineLogger : ILogger
    {
        private readonly Action<string> writer;

        public CommandLineLogger(Action<string> writer)
        {
            this.writer = writer;
        }
        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Debug(string message, params object[] paramList)
        {
            writer("[Debug]"+ message);
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="message">The message<see cref="ReadOnlyMemory{char}"/>.</param>
        public void Debug(ReadOnlyMemory<char> message)
        {
            writer("[Debug]" + message);
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Error(string message, params object[] paramList)
        {
            writer(string.Format("[Error]" + message, paramList));
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="message">The message<see cref="ReadOnlyMemory{char}"/>.</param>
        public void Error(ReadOnlyMemory<char> message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The ErrorException.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void ErrorException(string message, Exception exception, params object[] paramList)
        {
            writer(string.Format("[Error]" + message, paramList));
            writer(string.Format($"[Error] exception is {exception.Message}\n stack trace is {exception.StackTrace}"));
        }

        /// <summary>
        /// The Fatal.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Fatal(string message, params object[] paramList)
        {
            writer(string.Format("[Fatal]" + message, paramList));
        }

        /// <summary>
        /// The FatalException.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void FatalException(string message, Exception exception, params object[] paramList)
        {
            writer(string.Format("[Fatal]" + message, paramList));
            writer(string.Format($"[Fatal] exception is {exception.Message}\n stack trace is {exception.StackTrace}"));
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Info(string message, params object[] paramList)
        {
            writer(string.Format("[Info]" + message, paramList));
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="message">The message<see cref="ReadOnlyMemory{char}"/>.</param>
        public void Info(ReadOnlyMemory<char> message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="severity">The severity<see cref="LogSeverity"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Log(LogSeverity severity, string message, params object[] paramList)
        {
            writer(string.Format($"[{severity}]" + message, paramList));
        }

        /// <summary>
        /// The Log.
        /// </summary>
        /// <param name="severity">The severity<see cref="LogSeverity"/>.</param>
        /// <param name="message">The message<see cref="ReadOnlyMemory{char}"/>.</param>
        public void Log(LogSeverity severity, ReadOnlyMemory<char> message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The LogMultiline.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="severity">The severity<see cref="LogSeverity"/>.</param>
        /// <param name="additionalContent">The additionalContent<see cref="StringBuilder"/>.</param>
        public void LogMultiline(string message, LogSeverity severity, StringBuilder additionalContent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Warn(string message, params object[] paramList)
        {
            writer(string.Format($"[Warn]" + message, paramList));
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="message">The message<see cref="ReadOnlyMemory{char}"/>.</param>
        public void Warn(ReadOnlyMemory<char> message)
        {
            throw new NotImplementedException();
        }
    }
}
