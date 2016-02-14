using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Modding;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class CompileModTask : QueueableTask
    {
        private IModAsset _mod;
        private IModCompiler _modCompiler;
        public CompileModTask(IModAsset mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _modCompiler = ObjectFactory.GetInstance<IModCompiler>();
            _mod = mod;

            Name = "Compile mod";
        }
        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            UpdateStatus($"Compiling {_mod.Name}...", 25, TaskStatus.Running);

            var result = await _modCompiler.Compile(_mod);
            UpdateStatus($"Compiled {_mod.Name} with {result.Errors.Length} errors! Success? {result.Success}", 100, TaskStatus.Stopped);
        }

        #endregion
    }
}
