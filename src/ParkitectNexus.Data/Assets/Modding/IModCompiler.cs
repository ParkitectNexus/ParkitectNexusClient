using System.Threading.Tasks;

namespace ParkitectNexus.Data.Assets.Modding
{
    public interface IModCompiler
    {
        Task<ModCompileResults> Compile(IModAsset mod);
    }
}
