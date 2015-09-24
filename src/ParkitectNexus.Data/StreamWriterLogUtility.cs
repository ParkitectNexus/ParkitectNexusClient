// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexus.Data
{
    public static class StreamWriterLogUtility
    {
        public static void Log(this StreamWriter streamWriter, string message)
        {
            if (streamWriter == null) throw new ArgumentNullException(nameof(streamWriter));
            streamWriter.Log(message, LogLevel.Info);
        }

        public static void Log(this StreamWriter streamWriter, string message, LogLevel logLevel)
        {
            if (streamWriter == null) throw new ArgumentNullException(nameof(streamWriter));

            streamWriter.WriteLine($"[{DateTime.Now.ToString("G")}] {logLevel}: {message}");
        }
    }
}