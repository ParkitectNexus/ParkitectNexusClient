// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Reporting
{
    public class FailedCrashReport
    {
        public FailedCrashReport(string action, Exception exception, Exception crashReportFailureException)
        {
            Action = action;
            CrashReportFailureException = crashReportFailureException;
            Exception = exception;
        }

        public string Action { get; set; }
        public Exception Exception { get; set; }

        public Exception CrashReportFailureException { get; set; }
    }
}