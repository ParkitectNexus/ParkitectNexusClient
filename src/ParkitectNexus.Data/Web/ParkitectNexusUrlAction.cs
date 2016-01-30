// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Web
{
    public enum ParkitectNexusUrlAction
    {
        None,
        [ParkitectNexusUrlAction(typeof (InstallUrlAction))] Install,
        [ParkitectNexusUrlAction(typeof (AuthUrlAction))] Auth
    }
}
