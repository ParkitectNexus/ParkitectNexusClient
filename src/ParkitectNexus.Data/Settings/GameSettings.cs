// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace ParkitectNexus.Data.Settings
{
	[DataContract]
    public class GameSettings : SettingsBase
    {
		[DataMember(Name="install_path")]
        public string InstallationPath { get; set; }


    }
}