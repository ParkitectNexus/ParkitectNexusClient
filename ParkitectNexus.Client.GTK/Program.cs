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
using ParkitectNexus.Data.Reporting;
using System.Reflection;

namespace ParkitectNexus.Client.GTK
{
	
	public class MainClass
	{

		static void delete_event (object obj, DeleteEventArgs args)
		{
			
		}

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
			Parser.Default.ParseArguments(args, options);

			Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
			Log.MinimumLogLevel = options.LogLevel;

			Application.Init ();
		

			try
			{
				Log.WriteLine($"Application was launched with arguments '{string.Join(" ", args)}'.", LogLevel.Info);

				// Check for updates. If updates are available, do not resume usual logic.
				var updateInfo = ParkitectUpdate.CheckForUpdates(parkitectNexusWebsite);
				if (updateInfo != null)
				{
					ParkitectUpdate parkitectUpdate = new ParkitectUpdate(updateInfo,settings,options);
					parkitectUpdate.Show();
					if (parkitectUpdate.Run () == (int)Gtk.ResponseType.Close) {
						parkitectUpdate.Destroy ();
					}
				}
					
				
				if(OperatingSystems.GetOperatingSystem() == SupportedOperatingSystem.Windows)
					ParkitectNexusProtocol.Install();

				//find the new location of where parkitect is installed
				if (!parkitect.IsInstalled) {
					ParkitectFindError parkitectError = new ParkitectFindError (parkitect);
					parkitectError.Show ();
					if (parkitectError.Run () == (int)Gtk.ResponseType.Close) {
						parkitectError.Destroy ();
					}
				}

				// Ensure parkitect has been installed. If it has not been installed, quit the application.
				if(OperatingSystems.GetOperatingSystem() == SupportedOperatingSystem.Windows)
					ParkitectUpdate.MigrateMods(parkitect);

				ModLoaderUtility.InstallModLoader(parkitect);

				// Install backlog.
				if (!string.IsNullOrWhiteSpace(settings.DownloadOnNextRun))
				{
					Download(settings.DownloadOnNextRun, parkitect, parkitectOnlineAssetRepository);
					settings.DownloadOnNextRun = null;
					settings.Save();
				}

				// Process download option.
				if (options.DownloadUrl != null)
				{
					Download(options.DownloadUrl, parkitect, parkitectOnlineAssetRepository);
					return;
				}

				// If the launch option has been used, launch the game.
				if (options.Launch)
				{
					parkitect.Launch();
					return;
				}

				if (options.Silent && !settings.BootOnNextRun)
					return;

				settings.BootOnNextRun = false;
				settings.Save();

				MainWindow window = new MainWindow ();
				window.DeleteEvent += (o , arg) =>{
					// Handle silent calls.
					Log.Close();
			
				};
			
			

			}
			catch (Exception e)
			{
				Log.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
				Log.WriteException(e);
				CrashReporter.Report("global", parkitect, parkitectNexusWebsite, e);

				Gtk.MessageDialog err = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "The application has crashed in an unusual way.\n\nThe error has been logged to:\n"+ Log.LoggingPath);
				err.Run();

				Environment.Exit(0);
		

			}

			Application.Run ();
		}


		private static void Download(string url, IParkitect parkitect,
			IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
		{
			// Try to parse the specified download url. If parsing fails open ParkitectNexus. 
			ParkitectNexusUrl parkitectNexusUrl;
			if (!ParkitectNexusUrl.TryParse(url, out parkitectNexusUrl))
			{
				// Create a form to allow the dialogs to have a owner with forced focus and an icon.

				Gtk.MessageDialog errorDialog = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok,"The URL you opened is invalid!");
				errorDialog.Run ();
				errorDialog.Destroy();
				return;
			}

			// Run the download process in an installer form, for a nice visible process.
			//var form = new WizardForm();
			//form.Attach(new InstallAssetUserControl(parkitect, parkitectOnlineAssetRepository, parkitectNexusUrl, null));
			//Application.Run(form);
		}
	}
}
