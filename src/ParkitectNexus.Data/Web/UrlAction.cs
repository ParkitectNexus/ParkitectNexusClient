// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Web
{
    public enum UrlAction
    {
        None,
        [UrlAction(typeof (InstallUrlAction))] Install,
        [UrlAction(typeof (AuthUrlAction))] Auth
    }
}