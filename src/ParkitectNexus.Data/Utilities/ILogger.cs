// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Utilities
{
    public interface ILogger : IDisposable
    {
        /// <summary>
        ///     Gets or sets the minimum log level.
        /// </summary>
        LogLevel MinimumLogLevel { get; set; }

        /// <summary>
        ///     Gets the logging path.
        /// </summary>
        string LoggingPath { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is opened.
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        ///     Opens the logging stream at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        void Open(string path);

        /// <summary>
        ///     Closes the logging stream.
        /// </summary>
        void Close();

        /// <summary>
        ///     Writes the specified message to the log at log level <see cref="LogLevel.Debug" />.
        /// </summary>
        /// <param name="message">The message.</param>
        void WriteLine(string message);

        /// <summary>
        ///     Writes the specified message to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        void WriteLine(string message, LogLevel logLevel);

        /// <summary>
        ///     Writes the specified exception to the log at log level <see cref="LogLevel.Fatal" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void WriteException(Exception exception);

        /// <summary>
        ///     Writes the specified exception to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        void WriteException(Exception exception, LogLevel logLevel);
    }
}