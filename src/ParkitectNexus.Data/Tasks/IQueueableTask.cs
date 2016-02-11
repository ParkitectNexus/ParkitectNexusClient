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

        string StatusDescription { get; set; }

        TaskStatus Status { get; set; }

        int CompletionPercentage { get; set; }

        event EventHandler StatusChanged;

        Task Run(CancellationToken token);
    }
}