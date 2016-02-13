// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Contains StreamWriter logging utility methods.
    /// </summary>
    public static class StreamWriterLogUtility
    {
        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">streamWriter is null.</exception>
        public static void Log(this StreamWriter streamWriter, string message)
        {
            if (streamWriter == null) throw new ArgumentNullException(nameof(streamWriter));
            streamWriter.Log(message, LogLevel.Info);
        }

        /// <summary>
        ///     Logs the specified stream writer.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The log level.</param>
        /// <exception cref="ArgumentNullException">Thrown if streamWriter is null.</exception>
        public static void Log(this StreamWriter streamWriter, string message, LogLevel logLevel)
        {
            if (streamWriter == null) throw new ArgumentNullException(nameof(streamWriter));
            streamWriter.WriteLine($"[{DateTime.Now.ToString("yy-MM-dd HH:mm:ss")}] {logLevel}: {message}");
        }
    }
}