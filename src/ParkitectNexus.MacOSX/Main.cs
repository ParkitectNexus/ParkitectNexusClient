using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.MacOSX
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var registry = ObjectFactory.ConfigureStructureMap();

            registry.IncludeRegistry(new PresenterRegistry());
            ObjectFactory.SetUpContainer(registry);

            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}

