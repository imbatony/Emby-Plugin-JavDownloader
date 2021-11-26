// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" author="imbatony">
//     Copyright (c) JavDownloader.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace JavDownloader.Core.Logger
{
    using System;

    /// <summary>
    /// Defines the <see cref="ILogger" />.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// The ErrorException.
        /// </summary>
        /// <param name="message">The v<see cref="string"/>.</param>
        /// <param name="exception">The exception<see cref="Exception"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void ErrorException(string message, Exception exception, params object[] paramList);

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Error(string message, params object[] paramList);

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        void Debug(string message, params object[] paramList);
    }

    /// <summary>
    /// Defines the <see cref="CommandLineLogger" />.
    /// </summary>
    public class DefaultLogger : ILogger
    {
        /// <summary>
        /// The CreateDefaultLogger.
        /// </summary>
        /// <param name="writer">The writer<see cref="Action{string}"/>.</param>
        /// <returns>The <see cref="ILogger"/>.</returns>
        public static ILogger CreateDefaultLogger(Action<string> writer)
        {
            return new DefaultLogger(writer);
        }

        /// <summary>
        /// Defines the writer.
        /// </summary>
        private readonly Action<string> writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLogger"/> class.
        /// </summary>
        /// <param name="writer">The writer<see cref="Action{string}"/>.</param>
        public DefaultLogger(Action<string> writer)
        {
            this.writer = writer;
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
        /// <param name="message">The message<see cref="string"/>.</param>
        /// <param name="paramList">The paramList<see cref="object[]"/>.</param>
        public void Debug(string message, params object[] paramList)
        {
            writer(string.Format("[Debug]" + message, paramList));
        }
    }
}
