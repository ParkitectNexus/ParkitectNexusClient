// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Client.Base.Main;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base
{
    public class App : IPresenter
    {
        private readonly IPresenterFactory _presenterFactory;
        private readonly IParkitect _parkitect;

        public static UIImageProvider Images { get; } = new UIImageProvider();

        public App(IPresenterFactory presenterFactory, IParkitect parkitect)
        {
            _presenterFactory = presenterFactory;
            _parkitect = parkitect;
        }

        public void Run(ToolkitType type)
        {
            Application.Initialize(type);

            var window = _presenterFactory.InstantiatePresenter<MainWindow>();
            window.Show();

            if (!_parkitect.DetectInstallationPath())
            {
                if(!MessageDialog.Confirm("We couldn't detect Parkitect on your machine.\nPlease point me to it!", Command.Ok))
                {
                    window.Dispose();
                    Application.Dispose();
                    return;
                }

                do
                {
                    var dlg = new SelectFolderDialog("Select your Parkitect installation folder.")
                    {
                        CanCreateFolders = false,
                        Multiselect = false
                    };


                    if (dlg.Run())
                    {
                        if (_parkitect.SetInstallationPathIfValid(dlg.Folder))
                            break;
                    }
                    else
                    {
                        window.Dispose();
                        Application.Dispose();
                        return;
                    }
                } while (!_parkitect.IsInstalled);
            }


            Application.Run();

            window.Dispose();

            Application.Dispose();
        }
    }
}
