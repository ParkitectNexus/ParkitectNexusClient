using System;
using ParkitectNexus.Data.Settings;
using Newtonsoft.Json.Linq;

namespace ParkitectNexus.Client.GTK
{
	public class ClientSettings : SettingsBase
	{
		public bool BootOnNextRun{get;set;}
		public String DownloadOnNextRun{get;set;}


	
	}
}

