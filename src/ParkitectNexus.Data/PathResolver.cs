using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data
{
    public class PathResolver : IPathResolver
    {
        public string AppData()
        {

                var path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Assembly.GetEntryAssembly().GetName().Name);

                Directory.CreateDirectory(path);
                return path;
        }

        public bool IsParkitectInstalled()
        {
            throw new NotImplementedException();
        }

        public string ParkitectPath()
        {
            throw new NotImplementedException();
        }
    }
}
