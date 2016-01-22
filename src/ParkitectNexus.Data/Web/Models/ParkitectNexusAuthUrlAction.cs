// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Web.Models
{
    public class ParkitectNexusAuthUrlAction : IParkitectNexusUrlAction
    {
        public ParkitectNexusAuthUrlAction(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        public string Key { get; }
    }
}