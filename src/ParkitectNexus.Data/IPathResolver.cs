using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data
{
    public interface IPathResolver
    {
        string AppData();

        bool IsParkitectInstalled();
        string ParkitectPath();

    }
}
