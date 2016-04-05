// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Base.Pages
{
    public class SavegamesPageView : AssetsPageView
    {
        public SavegamesPageView(IParkitect parkitect, IPresenter parent) : base(parkitect, AssetType.Savegame, parent)
        {
        }

        #region Overrides of Widget

        /// <summary>
        ///     Gets or sets the name of this widget.
        /// </summary>
        /// <value>The widgets name.</value>
        /// <remarks>The name can be used to identify this widget by e.g. designers.</remarks>
        public override string Name { get; set; } = "Savegames";

        #endregion
    }
}
