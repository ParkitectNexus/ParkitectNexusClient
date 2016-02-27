using System.CodeDom.Compiler;

namespace ParkitectNexus.Data.Assets.Modding
{
    public struct ModCompileResults
    {
        public static ModCompileResults Successful { get; } = new ModCompileResults(null, true);

        public static ModCompileResults Failure { get; } = new ModCompileResults(null, false);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ModCompileResults(CompilerError[] errors, bool success)
        {
            Errors = errors;
            Success = success;
        }

        public bool Success { get; }
        public CompilerError[] Errors { get; }
    }
}
