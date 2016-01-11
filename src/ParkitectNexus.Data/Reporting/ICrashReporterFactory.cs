// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Reporting
{
    public interface ICrashReporterFactory
    {
        void Report(string action, Exception exception);
    }
}