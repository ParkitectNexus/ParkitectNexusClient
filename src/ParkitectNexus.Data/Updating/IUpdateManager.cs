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

using System.Threading.Tasks;

namespace ParkitectNexus.Data.Updating
{
    public interface IUpdateManager
    {
        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        Task<UpdateInfo> CheckForUpdates<TEntryPoint>();

        /// <summary>
        ///     Installs the specified update.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>true on success; false otherwise.</returns>
        bool InstallUpdate(UpdateInfo update);
    }
}