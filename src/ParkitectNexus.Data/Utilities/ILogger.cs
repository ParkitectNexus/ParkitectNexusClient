using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
