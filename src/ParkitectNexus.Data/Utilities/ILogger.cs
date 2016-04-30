// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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