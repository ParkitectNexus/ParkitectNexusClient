// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public abstract class UrlQueueableTask : QueueableTask
    {
        protected UrlQueueableTask(string name) : base(name)
        {
        }

        public IUrlAction Data { get; set; }
    }
}
