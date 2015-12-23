using System;
using Gtk;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Game.Windows;
using CommandLine;
using System.IO;
using System.Linq;

namespace ParkitectNexus.Client.GTK
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IParkitect parkitect;
			IParkitectNexusWebsite parkitectNexusWebsite;
			IParkitectOnlineAssetRepository parkitectOnlineAssetRepository;

			switch (OperatingSystems.GetOperatingSystem())
			{
			case SupportedOperatingSystem.Windows:
				parkitect = new WindowsParkitect();
				parkitectNexusWebsite = new ParkitectNexusWebsite();
				parkitectOnlineAssetRepository = new ParkitectOnlineAssetRepository(parkitectNexusWebsite);
				break;
			case SupportedOperatingSystem.Linux:
				parkitect = new LinuxParkitect ();
				parkitectNexusWebsite = new ParkitectNexusWebsite ();
				parkitectOnlineAssetRepository = new ParkitectOnlineAssetRepository (parkitectNexusWebsite);
				break;
			default:
				return;
			}
			var options = new CommandLineOptions();
			var settings = new ClientSettings();

			//missing method for LINQ
			//Parser.Default.ParseArguments(args, options);

			Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
			Log.MinimumLogLevel = options.LogLevel;

			Application.Init ();

			//verify the application is intalled
			if (!parkitect.IsInstalled) {
				ParkitectFindError parkitectError = new ParkitectFindError (parkitect);
				if (parkitectError.Run () == (int)Gtk.ResponseType.Close) {
					parkitectError.Destroy ();
				}
			} 

			//MainWindow win = new MainWindow ();
			//win.Show ();
		
			Application.Run ();


		}
	}
}
