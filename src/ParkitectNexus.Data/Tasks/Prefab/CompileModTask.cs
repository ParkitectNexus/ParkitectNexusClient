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

            var result = await _modCompiler.Compile(_mod);

            _modLoadOrderBuilder.Build();

            UpdateStatus($"Compiled {_mod.Name} with {result.Errors.Length} errors! Success? {result.Success}", 100,
                TaskStatus.Stopped);
        }

        #endregion
    }
}