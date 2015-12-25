using System;
using System.Reflection;
using Microsoft.Win32;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data;
using System.Diagnostics;
using System.IO;

namespace ParkitectNexus.Client.GTK
{
	public static class ParkitectNexusProtocol
	{
		/// <summary>
		///     Installs the parkitectnexus:// protocol.
		/// </summary>
		public static void Install()
		{
			var appPath = Assembly.GetEntryAssembly ().Location;
				switch (OperatingSystems.GetOperatingSystem ()) {
					case SupportedOperatingSystem.Windows:
						try {
							

							var parkitectNexus = Registry.CurrentUser?.CreateSubKey (@"Software\Classes\parkitectnexus");
							parkitectNexus?.SetValue ("", "ParkitectNexus Client");
							parkitectNexus?.SetValue ("URL Protocol", "");
							parkitectNexus?.CreateSubKey (@"DefaultIcon")?.SetValue ("", $"{appPath},0");
							parkitectNexus?.CreateSubKey (@"shell\open\command")?.SetValue ("", $"\"{appPath}\" --download \"%1\"");
						} catch (Exception e) {
							Log.WriteLine ("Failed to install parkitectnexus:// protocol.");
							Log.WriteException (e);
						}
						break;
			case SupportedOperatingSystem.Linux:

			//dosen't quite work
				var s = Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/applications/", "parkitectnexus.desktop");
				File.WriteAllText(s,$"[Destkop Entry]\n" +
							"Name=ParkitectNexusClient \n" +
							"Exec="+appPath+" %u --download \"%1\" \n" +
							"Terminal=false \n" +
							"Type=Application \n"+
							"MimeType=x-scheme-handler/parkitectnexus;");	
						break;
				}

		}
	}
}

