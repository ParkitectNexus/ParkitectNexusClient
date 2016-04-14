// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Base.Pages
{
    public class SavegamesPageView : AssetsPageView
    {
        public SavegamesPageView(IParkitect parkitect, IPresenter parent)
            : base(parkitect, AssetType.Savegame, parent, "Savegames")
        {
        }
    }
}
