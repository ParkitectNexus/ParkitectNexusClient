// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class SavegamesTabPage : AssetsTabPage
    {
        public SavegamesTabPage(IParkitect parkitect) : base(parkitect, AssetType.Savegame)
        {
            Text = "Savegames";
        }
    }
}
