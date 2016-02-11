// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Tasks.Prefab;

namespace ParkitectNexus.Data.Web.Models
{
    [UrlActionTask(typeof (InstallAssetTask))]
    public class InstallUrlAction : IUrlAction
    {
        public InstallUrlAction(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}