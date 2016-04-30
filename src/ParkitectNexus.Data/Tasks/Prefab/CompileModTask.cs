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
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Modding;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class CompileModTask : QueueableTask
    {
        private readonly IModAsset _mod;
        private readonly IModCompiler _modCompiler;
        private readonly IModLoadOrderBuilder _modLoadOrderBuilder;

        public CompileModTask(IModAsset mod, IModCompiler modCompiler, IModLoadOrderBuilder modLoadOrderBuilder)
            : base("Compile mod")
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;
            _modCompiler = modCompiler;
            _modLoadOrderBuilder = modLoadOrderBuilder;
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            UpdateStatus($"Compiling {_mod.Name}...", 25, TaskStatus.Running);

            // Compile the mod.
            var result = await _modCompiler.Compile(_mod);

            // Build the mod load order and store it to the load.dat file.
            _modLoadOrderBuilder.BuildAndStore();

            // Set the status to finished.
            if (result.Success)
            {
                UpdateStatus(
                    $"Successfuly compiled {_mod.Name}!", 100,
                    TaskStatus.Finished);
            }
            else
            {
                UpdateStatus(
                    $"Failed compiling {_mod.Name} with {result.Errors?.Length ?? 0} compile errors! View mod.log file for more info.",
                    100,
                    TaskStatus.FinishedWithErrors);
            }
        }

        #endregion
    }
}