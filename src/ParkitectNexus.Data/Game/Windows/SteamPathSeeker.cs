// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ParkitectNexus.Data.Game.Windows
{
    internal class SteamPathSeeker
    {
        private RegistryKey SteamKey => Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam")
                                        ?? Registry.CurrentUser.OpenSubKey("Software\\Wow6432Node\\Valve\\Steam")
                                        ?? Registry.LocalMachine.OpenSubKey("Software\\Valve\\Steam")
                                        ?? Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Valve\\Steam");

        public bool IsSteamVersionInstalled
        {
            get
            {
                var key = SteamKey.OpenSubKey($@"Apps\{Steam.AppId}")?.GetValue("Installed", null);
                return key != null && (int) key == 1;
            }
        }

        private string SteamFolder => SteamKey?.GetValue("SteamPath")?.ToString()
                                      ?? SteamKey?.GetValue("InstallPath")?.ToString();

        private IEnumerable<string> LibraryFolders
        {
            get
            {
                var steamFolder = SteamFolder;
                yield return steamFolder;

                var configFile = steamFolder + "\\config\\config.vdf";

                var regex = new Regex("BaseInstallFolder[^\"]*\"\\s*\"([^\"]*)\"");
                using (var reader = new StreamReader(configFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var match = regex.Match(line);
                        if (match.Success)
                        {
                            yield return Regex.Unescape(match.Groups[1].Value);
                        }
                    }
                }
            }
        }

        public string GetParkitectInstallationPath()
        {
            if (!IsSteamVersionInstalled)
                return null;

            return LibraryFolders
                .Select(p => Path.Combine(p, "SteamApps", "common", "Parkitect"))
                .FirstOrDefault(p => File.Exists(Path.Combine(p, "Parkitect.exe")));
        }
    }
}
