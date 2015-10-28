// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Logging utility.
    /// </summary>
    public static class Log
    {
        private static StreamWriter _streamWriter;

        /// <summary>
        ///     Gets or sets the minimum log level.
        /// </summary>
        public static LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

        /// <summary>
        ///     Gets the logging path.
        /// </summary>
        public static string LoggingPath { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is opened.
        /// </summary>
        public static bool IsOpened => _streamWriter != null;

        /// <summary>
        ///     Opens the logging stream at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void Open(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (IsOpened) Close();

            try
            {
                LoggingPath = path;
                _streamWriter = File.AppendText(path);
                _streamWriter.AutoFlush = true;
            }
            catch
            {
                LoggingPath = null;
            }
        }

        /// <summary>
        ///     Closes the logging stream.
        /// </summary>
        public static void Close()
        {
            if (IsOpened)
            {
                LoggingPath = null;
                _streamWriter.Flush();
                _streamWriter.Dispose();
                _streamWriter = null;
            }
        }

        /// <summary>
        ///     Writes the specified message to the log at log level <see cref="LogLevel.Debug" />.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void WriteLine(string message)
        {
            WriteLine(message, LogLevel.Debug);
        }

        /// <summary>
        ///     Writes the specified message to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteLine(string message, LogLevel logLevel)
        {
            if (logLevel >= MinimumLogLevel)
                _streamWriter.Log(message, logLevel);
        }

        /// <summary>
        ///     Writes the specified exception to the log at log level <see cref="LogLevel.Fatal" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void WriteException(Exception exception)
        {
            WriteException(exception, LogLevel.Fatal);
        }

        /// <summary>
        ///     Writes the specified exception to the log if the specified loglevel is at least <see cref="MinimumLogLevel" />.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logLevel">The log level.</param>
        public static void WriteException(Exception exception, LogLevel logLevel)
        {
            WriteLine("Exception: " + exception.Message, logLevel);
            WriteLine("StrackTrace: " + exception.StackTrace, logLevel);
            if (exception.InnerException != null && exception.InnerException != exception)
            {
                WriteLine("InnerException:", logLevel);
                WriteException(exception.InnerException, logLevel);
            }
        }
    }
}