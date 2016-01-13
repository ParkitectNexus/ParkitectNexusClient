﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Windows.TabPages
{
    public class BlueprintsTabPage : LoadableTilesTabPage, IPresenter
    {
        private readonly IParkitect _parkitect;

        public BlueprintsTabPage(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;

            Text = "Blueprints";
        }

        #region Overrides of LoadableTilesTabPage

        protected override bool ReloadOnEnter { get; } = true;

        #region Overrides of LoadableTilesTabPage

        protected override void ClearTiles()
        {
            foreach (var tile in Controls.OfType<MetroTile>().ToArray())
            {
                Controls.Remove(tile);
                tile.TileImage.Dispose();
            }
        }

        #endregion

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<MetroTile>();

                var current = 0;
                var fileCount = _parkitect.GetAssetCount(ParkitectAssetType.Blueprint);
                foreach (var bp in _parkitect.GetAssets(ParkitectAssetType.Blueprint))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    using (var bitmap = Image.FromStream(bp.Open()))
                    {
                        var name = bp.Name;

                        if (name != null)
                        {
                            var tileImage = new Bitmap(bitmap, 100, 100);

                            var tile = new MetroTile
                            {
                                Text = name,
                                TextAlign = ContentAlignment.BottomCenter,
                                Style = MetroColorStyle.Default,
                                TileImage = tileImage,
                                UseTileImage = true,
                                TileImageAlign = ContentAlignment.MiddleCenter
                            };

                            tile.Click +=
                                (sender, args) =>
                                    (FindForm() as MainForm)?.SpawnSliderPanel(new BlueprintSliderPanel(bp));
                            tiles.Add(tile);
                        }
                    }

                    UpdateLoadingProgress((current++*100)/fileCount);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
