// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Web.Models
{
    public class ParkitectNexusInstallUrlAction : IParkitectNexusUrlAction
    {
        public ParkitectNexusInstallUrlAction(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}