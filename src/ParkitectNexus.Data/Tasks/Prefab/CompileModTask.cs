// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;
            _modCompiler = modCompiler;
            _modLoadOrderBuilder = modLoadOrderBuilder;

            Name = "Compile mod";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            UpdateStatus($"Compiling {_mod.Name}...", 25, TaskStatus.Running);

            // Compile the mod.
            var result = await _modCompiler.Compile(_mod);

            // Build the mod load order and store it to the load.dat file.
            _modLoadOrderBuilder.Build();

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
                    $"Failed compiling {_mod.Name} with {result.Errors?.Length ?? 0} compile errors! View mod.log file for more info.", 100,
                    TaskStatus.FinishedWithErrors);
            }
        }

        #endregion
    }
}
