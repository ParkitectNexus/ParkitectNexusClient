// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Diagnostics;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Web;
using Xwt;

namespace ParkitectNexus.Client.Base.Pages
{
    public class ModsPageView : AssetsPageView
    {
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IWebsite _website;

        public ModsPageView(IParkitect parkitect, IPresenter parent, IQueueableTaskManager queueableTaskManager,
            IWebsite website) : base(parkitect, AssetType.Mod, parent, "Mods")
        {
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
                Active = mod.Information.IsEnabled
            };
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

            var viewOnWebsiteButton = new Button("View on ParkitectNexus");
            viewOnWebsiteButton.Clicked +=
                (sender, args) => { Process.Start(_website.ResolveUrl($"redirect/{mod.Repository}", "client")); };

            var recompileButton = new Button("Recompile");
            recompileButton.Clicked += (sender, args) =>
            {
                _queueableTaskManager.With(mod).Add<CompileModTask>();
                MainView.SwitchToTab(4);
            };

            var updateButton = new Button("Update");
            updateButton.Clicked += (sender, args) =>
            {
                _queueableTaskManager.With(mod).Add<UpdateModTask>();
                MainView.SwitchToTab(4);
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
