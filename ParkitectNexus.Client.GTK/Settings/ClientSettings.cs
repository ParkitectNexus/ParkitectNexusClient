using System;
using ParkitectNexus.Data.Settings;

namespace ParkitectNexus.Client.GTK
{
	public class ClientSettings : SettingsBase
	{
		public bool BootOnNextRun{get;set;}
		public String DownloadOnNextRun{get;set;}
	}
}

