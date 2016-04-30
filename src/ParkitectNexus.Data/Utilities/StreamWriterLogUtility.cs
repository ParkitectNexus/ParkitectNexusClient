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