// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace ParkitectNexus.Data.Game.Windows
{
    internal class SteamPathSeeker
    {
        private static readonly string[] IgnoredFolders =
        {
            @"C:\Users",
            @"C:\Windows",
            @"C:\ProgramData",
            @"C:\$recycle.bin"
        };


        public bool IsSteamVersionInstalled
        {
            get
            {
                var key = Registry.GetValue($@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\Apps\{Steam.AppId}", "Installed",
                    null);
                return key != null && (int) key == 1;
            }
        }

        public string GetSteamInstallationPath()
        {
            if (!IsSteamVersionInstalled)
                return null;

            return GetSteamAppsFolders()
                .Select(p => Path.Combine(p, "common", "Parkitect"))
                .FirstOrDefault(p => File.Exists(Path.Combine(p, "Parkitect.exe")));
        }

        private IEnumerable<string> GetSteamAppsFolders()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed)
                .SelectMany(d => GetSteamAppsFolders(d.RootDirectory.FullName, 0));
        }

        private IEnumerable<string> GetSteamAppsFolders(string path, int depth)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(path);
            }
            catch
            {
                yield break;
            }

            foreach (var directory in directories.Except(IgnoredFolders))
            {
                if (Path.GetFileName(directory) == "SteamApps")
                    yield return directory;

                if (depth < 6)
                    foreach (var r in GetSteamAppsFolders(directory, depth + 1))
                        yield return r;
            }
        }
    }
}
