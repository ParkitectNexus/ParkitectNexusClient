// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Main;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Updating;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;
using Xwt;

namespace ParkitectNexus.Client.Base
{
    public class App : IPresenter
    {
        private readonly ILogger _log;
        private readonly IParkitect _parkitect;
        private readonly IPresenterFactory _presenterFactory;
        private readonly IQueueableTaskManager _taskManager;
        private readonly IUpdateManager _updateManager;
        private bool _isRunning;
        private readonly Migrator _migrator;
        private MainWindow _window;

        public App(IPresenterFactory presenterFactory, IParkitect parkitect, IQueueableTaskManager taskManager,
            ILogger log, IUpdateManager updateManager, Migrator migrator)
        {
            _migrator = migrator;
            _presenterFactory = presenterFactory;
            _parkitect = parkitect;
            _taskManager = taskManager;
            _log = log;
            _updateManager = updateManager;
        }

        public static UIImageProvider Images { get; } = new UIImageProvider();


        public bool Initialize(ToolkitType type)
        {
            _log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            _log.MinimumLogLevel = LogLevel.Debug;

            Application.Initialize(type);

            _window = _presenterFactory.InstantiatePresenter<MainWindow>();
            _window.Show();

            var update = _updateManager.CheckForUpdates<App>();
            if (update != null)
            {
                if (
                    MessageDialog.AskQuestion(
                        "A required update for the ParkitectNexus Client needs to be installed.\n" +
                        "Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n" +
                        $"Would you like to install it now?\n\nYou are currently running v{typeof (App).Assembly.GetName().Version}. The newest version is v{update.Version}",
                        "ParkitectNexus Client Update", Command.Yes, Command.No) !=
                    Command.Yes)
                {
                    return false;
                }

                if (!_updateManager.InstallUpdate(update))
                    MessageDialog.ShowError(_window, "Failed installing the update! Please try again later.",
                        "ParkitectNexus Client Update");

                return false;
            }

            if (!_parkitect.DetectInstallationPath())
            {
                if (
                    !MessageDialog.Confirm(
                        "We couldn't automatically detect Parkitect on your machine!\nPlease press OK and manually select the installation folder of Parkitect.",
                        Command.Ok))
                {
                    _window.Dispose();
                    Application.Dispose();
                    return false;
                }

                do
                {
                    var dlg = new SelectFolderDialog("Select your Parkitect installation folder.")
                    {
                        CanCreateFolders = false,
                        Multiselect = false
                    };


                    if (dlg.Run(_window))
                    {
                        if (_parkitect.SetInstallationPathIfValid(dlg.Folder))
                            break;
                    }
                    else
                    {
                        _window.Dispose();
                        Application.Dispose();
                        return false;
                    }
                } while (!_parkitect.IsInstalled);
            }

            _migrator.Migrate();
            ModLoaderUtil.InstallModLoader(_parkitect, _log);

            return true;
        }

        public void HandleUrl(INexusUrl url)
        {
            var attribute = url.Data.GetType().GetCustomAttribute<UrlActionTaskAttribute>();
            if (attribute?.TaskType != null && typeof (UrlQueueableTask).IsAssignableFrom(attribute.TaskType))
            {
                var task = ObjectFactory.GetInstance<UrlQueueableTask>(attribute.TaskType);
                task.Data = url.Data;

                _taskManager.Add(task);
            }
        }

        private async void CheckForIpcFile()
        {
            var ipcPath = Path.Combine(AppData.Path, "ipc.dat");
            while (_isRunning)
            {
                try
                {
                    if (File.Exists(ipcPath))
                    {
                        var lines = File.ReadAllLines(ipcPath);
                        File.Delete(ipcPath);
                        foreach (var contents in lines)
                        {
                            NexusUrl url;
                            if (NexusUrl.TryParse(contents, out url))
                                HandleUrl(url);
                        }
                    }
                }
                catch
                {
                }
                await Task.Delay(1000);
            }
        }

        public void Run()
        {
            _isRunning = true;
            CheckForIpcFile();

            Application.Run();

            _isRunning = false;
            _window.Dispose();
            Application.Dispose();
        }
    }
}