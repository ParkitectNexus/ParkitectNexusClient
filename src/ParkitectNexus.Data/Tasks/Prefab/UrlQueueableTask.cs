// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public abstract class UrlQueueableTask : QueueableTask
    {
        public IUrlAction Data { get; set; }
    }
}