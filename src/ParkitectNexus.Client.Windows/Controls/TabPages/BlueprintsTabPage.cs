// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class BlueprintsTabPage : AssetsTabPage
    {
        public BlueprintsTabPage(IParkitect parkitect) : base(parkitect, AssetType.Blueprint)
        {
            Text = "Blueprints";
        }
    }
}
