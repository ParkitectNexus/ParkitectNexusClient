// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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