using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Client.Base;
using Xwt;

namespace ParkitectNexus.Client.Darwin
{
    class MainClass
    {
        static void Main(string[] args)
        {
            var registry = ObjectFactory.ConfigureStructureMap();
            registry.IncludeRegistry(new PresenterRegistry());
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();

            var app = presenterFactory.InstantiatePresenter<App>();
            app.Initialize(ToolkitType.Cocoa);

            app.Run();
        }
    }
}

