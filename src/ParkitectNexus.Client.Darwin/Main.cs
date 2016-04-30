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

namespace ParkitectNexus.Client.Darwin
{
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

