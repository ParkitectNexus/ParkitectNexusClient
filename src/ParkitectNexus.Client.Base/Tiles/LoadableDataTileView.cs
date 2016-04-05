// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data.Presenter;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Tiles
{
    public abstract class LoadableDataTileView : ScrollView, IPresenter
    {
        private readonly VBox _box;

        private readonly List<Button> _buttons = new List<Button>();
        private readonly Stack<HBox> _rows = new Stack<HBox>();
        private int _buttonsPerRow;
        private Size _tileSize = new Size(100, 100);
        private CancellationTokenSource _tokenSource;

        protected LoadableDataTileView()
        {
            Content = _box = new VBox();
            PushNewRow();
            RefreshTiles();
        }

        private void PushNewRow()
        {
            var v = new HBox {Margin = new WidgetSpacing(5, 5, 5, 5)};
            _rows.Push(v);
            _box.PackStart(v);
        }

        protected virtual void ClearTiles()
        {
            foreach (var r in _rows)
                r.Clear();

            _rows.Clear();
            _box.Clear();
            PushNewRow();
        }

        protected abstract Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken);

        private int CalculateButtonsPerRow()
        {
            return Math.Max(1,
                (int) Math.Floor((Size.Width - 5 - 25 /*scroll and a bit*/)/(_tileSize.Width + 5)));
        }

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


            Spinner spinner = null;

            Application.Invoke(() =>
            {
                Content =
                    spinner = new Spinner
                    {
                        Animate = true,
                        MinHeight = 50,
                        MinWidth = 50
                    };
            });

            while (spinner == null)
                await Task.Delay(5);

            _buttonsPerRow = CalculateButtonsPerRow();
            var i = 0;

            _tokenSource = new CancellationTokenSource();

            try
            {
                var tiles = await LoadTiles(_tokenSource.Token);

                Application.Invoke(() =>
                {
                    foreach (var tile in tiles)
                    {
                        if (tile == null) continue;

                        if (i >= _buttonsPerRow)
                        {
                            PushNewRow();
                            i = 0;
                        }
                        var button = new Button(tile.Image?.ToXwtImage()?.WithSize(100))
                        {
                            Label = tile.Image == null ? tile.Text : null,
                            TooltipText = tile.Text,
                            WidthRequest = 100,
                            HeightRequest = 100,
                            MinWidth = 0,
                            Style = ButtonStyle.Borderless,
                            BackgroundColor = Color.FromBytes(45, 137, 239),
                            ImagePosition = ContentPosition.Center
                        };

                        button.Clicked += (sender, args) => tile.ClickAction();

                        _buttons.Add(button);
                        _rows.Peek().PackStart(button);
                        i++;
                    }
                    Content = _box;
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
        }

        #region Overrides of Widget

        /// <summary>
        ///     Raises the bounds changed event.
        /// </summary>
        /// <remarks>
        ///     Override <see cref="OnBoundsChanged" /> to handle the event internally and call the base
        ///     <see cref="Xwt.Widget.OnBoundsChanged" /> to finally raise the event.
        ///     The event will be enabled in the backend automatically, if <see cref="Xwt.Widget.OnBoundsChanged" />
        ///     is overridden.
        /// </remarks>
        protected override void OnBoundsChanged()
        {
            base.OnBoundsChanged();

            if (CalculateButtonsPerRow() == _buttonsPerRow)
                return;

            _buttonsPerRow = CalculateButtonsPerRow();
            var i = 0;

            ClearTiles();
            PushNewRow();

            foreach (var button in _buttons)
            {
                if (i >= _buttonsPerRow)
                {
                    PushNewRow();
                    i = 0;
                }
                _rows.Peek().PackStart(button);
                i++;
            }
        }

        #endregion
    }
}
