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

namespace ParkitectNexus.Data.Assets.Modding
{
    public class ModInformation
    {
        /// <summary>
        ///     Gets or sets the base directory.
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        ///     Gets or sets the compiler version.
        /// </summary>
        public string CompilerVersion { get; set; } = "v3.5";

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is development.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the project.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        ///     Gets or sets the dependencies.
        /// </summary>
        public string[] Dependencies { get; set; }
    }
}