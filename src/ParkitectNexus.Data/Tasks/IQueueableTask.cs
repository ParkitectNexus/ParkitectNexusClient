// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Tasks
{
    public interface IQueueableTask
    {
        string Name { get; }
        string StatusDescription { get; }
        TaskStatus Status { get; }
        int CompletionPercentage { get; }
        event EventHandler StatusChanged;
        Task Run(CancellationToken token);
    }
}
