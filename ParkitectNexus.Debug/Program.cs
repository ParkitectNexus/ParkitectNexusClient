using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Debug
{
    internal class Program
    {
        class CacheDat
        {
            public string Name { get; set; }
            public ICachedFile File { get; set; } = new CachedFile();
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static void Main(string[] args)
        {

            //configure map
            var registry = ObjectFactory.ConfigureStructureMap();
            registry.IncludeRegistry(new PresenterRegistry());
            ObjectFactory.SetUpContainer(registry);

            IParkitect p = ObjectFactory.GetInstance<IParkitect>();

            p.SetInstallationPathIfValid(@"C:\Users\Tim\Desktop\Parkitect_Pre-Alpha_6b_64bit");
            ILocalAssetsRepository localAssetsRepository = ObjectFactory.GetInstance<ILocalAssetsRepository>();

            var bps = localAssetsRepository.GetAssets(AssetType.Savegame);

            foreach (var bp in bps)
            {
                Console.WriteLine($"{bp.InstallationPath}: {bp.Name}");
            }
        }
    }
}
