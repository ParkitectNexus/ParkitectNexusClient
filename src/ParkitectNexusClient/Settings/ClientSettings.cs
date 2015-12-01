// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using ParkitectNexus.Data.Settings;

namespace ParkitectNexus.Client.Settings
{
    public class ClientSettings : SettingsBase
    {
        public bool BootOnNextRun { get; set; }
        public string DownloadOnNextRun { get; set; }
    }
}