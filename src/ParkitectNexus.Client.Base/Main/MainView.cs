// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainView : VBox, IPresenter
    {
        public MainView(IPresenterFactory presenterFactory)
        {
            var tabcon = presenterFactory.InstantiatePresenter<MainNotebook>();
            tabcon.Add(presenterFactory.InstantiatePresenter<MenuPageView>());
            tabcon.Add(new Tmptmp("Mods"));
            tabcon.Add(new Tmptmp("Blueprints"));
            tabcon.Add(new Tmptmp("Savegames"));
            tabcon.Add(presenterFactory.InstantiatePresenter<SavegamesTileView>());
            tabcon.Add(new Tmptmp("Tasks"));
            tabcon.Add(new FooView());
//            tabcon.Add(new Label("Menu placeholder"), "Menu");
//            tabcon.Add(new Label("Mods placeholder"), "Mods");
//            tabcon.Add(new Label("Blueprints placeholder"), "Blueprints");
//            tabcon.Add(new Label("Savegames placeholder"), "Savegames");
//            tabcon.Add(new Label("Tasks placeholder"), "Tasks");
            PackStart(presenterFactory.InstantiatePresenter<MainHeaderView>());
            PackEnd(tabcon, true, true);
        }

        private class Tmptmp : VBox
        {
            public Tmptmp(string name)
            {
                Name = name;
            }
        }
    }

    public class FooView : LoadableDataTileView
    {
        #region Overrides of Widget

        /// <summary>
        ///     Gets or sets the name of this widget.
        /// </summary>
        /// <value>The widgets name.</value>
        /// <remarks>The name can be used to identify this widget by e.g. designers.</remarks>
        public override string Name { get; set; } = "Foo Test";

        #endregion

        #region Overrides of LoadableDataTileView

        protected override Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken)
        {
            var l = new List<Tile>();

            for (var i = 0; i < 5; i++)
            {
                l.Add(new Tile(null, "Thumbs up xyz xyz xyz xyz" + i, () => { }));
            }

            return Task.FromResult((IEnumerable<Tile>) l);
        }

        #endregion
    }

    public class SavegamesTileView : AssetsTileView
    {
        public SavegamesTileView(IParkitect parkitect) : base(parkitect, AssetType.Savegame)
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

    public class AssetsTileView : LoadableDataTileView
    {
        private readonly IParkitect _parkitect;
        private readonly AssetType _type;

        public AssetsTileView(IParkitect parkitect, AssetType type)
        {
            _parkitect = parkitect;
            _type = type;
        }

        #region Overrides of LoadableDataTileView

        protected override Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<Tile>();

                if(_parkitect == null || _parkitect.Assets == null)
                {
                    return (IEnumerable<Tile>)new Tile[0];
                }

                var current = 0;
                var fileCount = _parkitect.Assets.GetAssetCount(_type);
                foreach (var asset in _parkitect.Assets[_type])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tile = new Tile(asset.GetImage(), asset.Name, () => { });
                    tiles.Add(tile);

                    UpdateLoadingProgress((current++*100)/fileCount);
                }
                return (IEnumerable<Tile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }

    public class Tile
    {
        public System.Drawing.Image Image { get; }

        public string Text { get; }

        public Action ClickAction { get; }

        public Tile(System.Drawing.Image image, string text, Action clickAction)
        {
            if (clickAction == null) throw new ArgumentNullException(nameof(clickAction));
            Image = image;
            Text = text;
            ClickAction = clickAction;
        }
    }

    public abstract class LoadableDataTileView : VBox, IPresenter
    {
        private readonly Stack<HBox> _rows = new Stack<HBox>();
        private CancellationTokenSource _tokenSource;

        protected LoadableDataTileView()
        {
            var v = new HBox();
            v.Margin = new WidgetSpacing(5, 5, 5, 5);
            _rows.Push(v);
            PackStart(v);

            RefreshTiles();
        }


        protected virtual void ClearTiles()
        {
            //throw new NotImplementedException();
        }

        protected void UpdateLoadingProgress(int percentage)
        {
            var value = Math.Min(100, Math.Max(0, percentage));

            // TODO: Update spinner
        }

        protected abstract Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken);

        public async void RefreshTiles()
        {
            // Cancel previous loads
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();

                while (_tokenSource != null)
                    await Task.Delay(1);
            }

            // Clear controls
            ClearTiles();

            // Add load spinner
            UpdateLoadingProgress(0);
            // TODO: Add spinner

            const int marginX = 5;
            const int marginY = 5;
            var size = new Size(100, 100);
            var countX = (Size.Width - marginX - 0 /*vertical scroll bar width*/)/(size.Width + marginX);

            if (countX < 1) countX = 1;

            var i = 0;

            _tokenSource = new CancellationTokenSource();

            try
            {
                var tiles = await LoadTiles(_tokenSource.Token);

                Application.Invoke(() =>
                {
                    ;
                    foreach (var tile in tiles)
                    {
                        if (tile == null) continue;

                        var x = i%countX;
                        var y = i/countX;
                        // tile.Location = new Point(Margin.Left + (x * (size.Width + marginX)),
                        //     Margin.Top + (y * (size.Height + marginY)));
                        // tile.Size = size;
                        //  Controls.Add(tile);
                        // TODO: Add
                        _rows.Peek().PackStart(new Button(tile.Image?.ToXwtImage()?.WithSize(100))
                        {
                            Label = tile.Image == null ? tile.Text : null,
                            TooltipText = tile.Text,
                            WidthRequest = 100,
                            HeightRequest = 100,
                            Style = ButtonStyle.Borderless,
                            BackgroundColor = Color.FromBytes(45, 137, 239),
                            ImagePosition = ContentPosition.Center,
                        });
                        i++;
                    }
                });
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }

            // TODO: Remove spinner
        }
    }
}