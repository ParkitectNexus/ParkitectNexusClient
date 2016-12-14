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

using System.Diagnostics;
using System.Linq;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;

namespace ParkitectNexus.Client.Base.Pages
{
    public class ModsPageView : AssetsPageView
    {
        private readonly IParkitect _parkitect;
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IWebsite _website;

        public ModsPageView(IParkitect parkitect, ILogger log, IPresenter parent, IQueueableTaskManager queueableTaskManager,
            IWebsite website) : base(parkitect, website, log, AssetType.Mod, parent, "Mods")
        {
            _parkitect = parkitect;
            _queueableTaskManager = queueableTaskManager;
            _website = website;
        }

        #region Overrides of AssetsPageView

        protected override void PopulateViewBoxWithTitle(VBox vBox, IAsset asset)
        {
            base.PopulateViewBoxWithTitle(vBox, asset);

            var mod = asset as IModAsset;

            var version = new HBox();
            version.PackStart(new Label("Version"));
            version.PackEnd(new Label(mod.Information.IsDevelopment ? "IN DEVELOPMENT" : mod.Tag)
            {
                TextAlignment = Alignment.End
            });

            var enabled = new HBox();
            enabled.PackStart(new Label("Enabled"));

            var enabledCheckbox = new CheckBox
            {
                Active = mod.Information.IsEnabled,
                Sensitive = !mod.Information.IsDevelopment
            };

            if (!mod.Information.IsDevelopment)
                enabledCheckbox.Clicked += (sender, args) =>
                {
                    mod.Information.IsEnabled = !mod.Information.IsEnabled;
                    mod.SaveInformation();
                };

            enabled.PackEnd(enabledCheckbox);

            vBox.PackStart(version);
            vBox.PackStart(enabled);
        }

        protected override void PopulateViewBoxWithButtons(VBox vBox, IAsset asset)
        {
            var mod = asset as IModAsset;

            var viewOnWebsiteButton = new Button("View on ParkitectNexus")
            {
                Sensitive = !mod.Information.IsDevelopment
            };
            if (!mod.Information.IsDevelopment)
                viewOnWebsiteButton.Clicked +=
                    (sender, args) => { Process.Start(_website.ResolveUrl($"redirect/{mod.Repository}", "client")); };

            var recompileButton = new Button("Recompile");
            recompileButton.Clicked += (sender, args) =>
            {
                _queueableTaskManager.With(mod).Add<CompileModTask>();
                MainView.SwitchToTab(5);
            };

            var updateButton = new Button("Update")
            {
                Sensitive = !mod.Information.IsDevelopment
            };
            if (!mod.Information.IsDevelopment)
                updateButton.Clicked += (sender, args) =>
                {
                    _queueableTaskManager.With(mod).Add<UpdateModTask>();
                };

            var utils = new HBox();
            utils.PackStart(recompileButton, true);
            utils.PackStart(updateButton, true);

            vBox.PackStart(viewOnWebsiteButton);
            vBox.PackStart(utils);

            base.PopulateViewBoxWithButtons(vBox, asset);
        }

        #endregion
    }
}
