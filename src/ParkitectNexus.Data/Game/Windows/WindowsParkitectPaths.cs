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

namespace ParkitectNexus.Data.Game.Windows
{
    /// <summary>
    ///     Represents a paths collection for the Windows version of the Parkitect game.
    /// </summary>
    public class WindowsParkitectPaths : BaseParkitectPaths
    {
        public WindowsParkitectPaths(IParkitect parkitect) : base(parkitect)
        {
        }

        public override string Data => GetPathInGameFolder("Parkitect_Data");

        public override string DataManaged => GetPathInGameFolder(@"Parkitect_Data\Managed");

        public override string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            if (!Parkitect.IsInstalled)
                return null;
            string documentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Parkitect");

            path = path == null
                ? documentsFolder
                : Path.Combine(documentsFolder, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}