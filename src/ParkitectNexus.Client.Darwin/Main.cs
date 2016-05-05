using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Client.Base;
using Xwt;
using Xwt.Mac;
using ParkitectNexus.Data.Web;
using System.Text.RegularExpressions;
using ParkitectNexus.Data.Game;
using System.IO;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Darwin
{
    public static class TmpFixModLoaderUtil
    {
        private static void InstallModLoaderFile(IParkitect parkitect, string fileName, string exten)
        {
            var targetDirectory = parkitect.Paths.NativeMods;

            Directory.CreateDirectory(targetDirectory);
            var targetPath = Path.Combine(targetDirectory, fileName + "." + exten);

            var sourcePath = NSBundle.MainBundle.PathForResource(fileName, exten);

            if (!File.Exists(sourcePath))
                return;

            if (File.Exists(targetPath))
            {
                using (var stream = File.OpenRead(sourcePath))
                using (var stream2 = File.OpenRead(targetPath))
                    if (stream.CreateMD5Checksum() == stream2.CreateMD5Checksum())
                        return;
            }

            File.Copy(sourcePath, targetPath, true);
        }

        public static void InstallModLoader(IParkitect parkitect, ILogger logger)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            try
            {
                InstallModLoaderFile(parkitect, "ParkitectNexus.Mod.ModLoader", "dll");
            }
            catch (Exception e)
            {
                logger.WriteLine("Failed to install mod loader.", LogLevel.Warn);
                logger.WriteException(e, LogLevel.Warn);
            }
        }
    }
    class MainClass
    {
        static void Main(string[] args)
        {
            var registry = ObjectFactory.ConfigureStructureMap();
            registry.IncludeRegistry(new PresenterRegistry());
            registry.For<IApp>().Singleton().Use<App>();
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();

            var app = presenterFactory.InstantiatePresenter<App>();
            if(!app.Initialize(ToolkitType.Cocoa))
                return;

            TmpFixModLoaderUtil.InstallModLoader(ObjectFactory.GetInstance<IParkitect>(), ObjectFactory.GetInstance<ILogger>());
            MacEngine.App.OpenUrl += (sender, e) =>
            {
                if(e.Url.StartsWith("parkitectnexus://"))
                    e.Url = e.Url;
                else
                {
                    var match = Regex.Match(e.Url, "<NSAppleEventDescriptor: \"(parkitectnexus:\\/\\/.*)\">");
                    if(match.Success)
                    {
                        e.Url = match.Groups[1].Value;
                    }
                }
                NexusUrl url;

                if(NexusUrl.TryParse(e.Url, out url))
                    app.HandleUrl(url);
            };

            app.Run();
        }
    }
}

