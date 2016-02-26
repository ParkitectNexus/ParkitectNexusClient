// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Tasks
{
    public enum TaskStatus
    {
        Queued,
        Running,
        Finished,
        FinishedWithErrors,
        Break,
        Canceled
    }
}
