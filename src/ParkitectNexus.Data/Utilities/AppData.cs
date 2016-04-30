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
using System.Reflection;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Contains the path to the app data storage.
    /// </summary>
    public static class AppData
    {
        /// <summary>
        ///     Gets the path to the app data storage.
        /// </summary>
        public static string Path
        {
            get
            {
                var path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Assembly.GetEntryAssembly().GetName().Name);

                Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}