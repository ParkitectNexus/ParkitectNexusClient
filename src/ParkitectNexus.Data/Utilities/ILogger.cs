// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Utilities
{
    public interface ILogger : IDisposable
    {
        LogLevel MinimumLogLevel { get; set; }
        string LoggingPath { get; }
        bool IsOpened { get; }

        void Open(string path);
        void Close();

        void WriteLine(string message);
        void WriteLine(string message, LogLevel logLevel);

        void WriteException(Exception exception);
        void WriteException(Exception exception, LogLevel logLevel);
    }
}