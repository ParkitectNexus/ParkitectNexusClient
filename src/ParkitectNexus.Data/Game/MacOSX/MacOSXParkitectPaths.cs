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

using System;
using System.IO;
using ParkitectNexus.Data.Game.Base;

namespace ParkitectNexus.Data.Game.MacOSX
{
    /// <summary>
    ///     Represents a paths collection for the MacOSX version of the Parkitect game.
    /// </summary>
    public class MacOSXParkitectPaths : BaseParkitectPaths
    {
        public MacOSXParkitectPaths(MacOSXParkitect parkitect) : base(parkitect)
        {
        }

        public override string Data => GetPathInGameFolder("Contents/Resources/Data");

        public override string DataManaged => GetPathInGameFolder("Contents/Resources/Data/Managed");

        public override string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            if (!Parkitect.IsInstalled)
                return null;

            var parkitectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Library/Application Support/Parkitect");

            path = path == null
                ? parkitectFolder
                : Path.Combine(parkitectFolder, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}