using System;
using ParkitectNexus.Data.Bases;
using System.IO;

namespace ParkitectNexus.Data
{
	public class LinuxParkitectPath : BaseParkitectPaths
	{
		public LinuxParkitectPath(LinuxParkitect parkitect) : base(parkitect)
		{
		}

		public override string Data => GetPathInGameFolder("Parkitect_Data");
		public override string DataManaged => GetPathInGameFolder(@"Parkitect_Data\Managed");

		public override string GetPathInSavesFolder(string path, bool createIfNotExists)
		{
			if(!Parkitect.IsInstalled)
				path = null;
			else if(path == null)
				path = Path.Combine(Parkitect.InstallationPath);
			else
				path = Path.Combine(Parkitect.InstallationPath, path);

			/*path = !Parkitect.IsInstalled
                ? null
                : path == null
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        "Library/Application Support/Parkitect")
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        "Library/Application Support/Parkitect", path);*/

			if (path != null && createIfNotExists)
				Directory.CreateDirectory(path);

			return path;
		}
	}
}

