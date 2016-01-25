using System.IO;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Caching
{
    public interface ICachedFile
    {
        bool Exists { get; }
        Stream Open(FileMode mode);
        void Delete();
    }
}
