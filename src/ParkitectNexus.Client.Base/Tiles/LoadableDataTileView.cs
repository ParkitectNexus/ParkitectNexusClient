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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;
using Xwt;
using ParkitectNexus.Data;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Tiles
{
    public abstract class LoadableDataTileView : ScrollView, IPresenter, IPageView
    {
        private readonly ILogger _log;
        private readonly VBox _box;

        private readonly List<Widget> _buttons = new List<Widget> ();
        private readonly Stack<HBox> _rows = new Stack<HBox> ();
        private int _buttonsPerRow;
        private Size _tileSize = new Size (100, 100);
        private CancellationTokenSource _tokenSource;

        protected LoadableDataTileView (ILogger log, string displayName)
        {
            _log = log;
            if (displayName == null)
                throw new ArgumentNullException (nameof (displayName));
            DisplayName = displayName;

            Content = _box = new VBox ();
            PushNewRow ();
            RefreshTiles ();
        }

        private void PushNewRow ()
        {
            var v = new HBox { Margin = new WidgetSpacing (5, 5, 5, 5) };
            _rows.Push (v);
            _box.PackStart (v);
        }

        protected virtual void ClearTiles ()
        {
            foreach (var r in _rows)
                r.Clear ();

            _rows.Clear ();
            _box.Clear ();
            PushNewRow ();
        }

        protected abstract Task<IEnumerable<Tile>> LoadTiles (CancellationToken cancellationToken);

        private int CalculateButtonsPerRow (float width)
        {
            return Math.Max (1,
                (int)Math.Floor ((width - 5 - 25 /*scroll and a bit*/) / (_tileSize.Width + 5)));
        }

        public async void RefreshTiles ()
        {
            // Cancel previous loads
            if (_tokenSource != null) {
                _tokenSource.Cancel ();

                while (_tokenSource != null)
                    await Task.Delay (1);
            }

            Spinner spinner = null;

            Application.Invoke (() => {
                // Clear controls
                _buttons.Clear ();
                ClearTiles ();

                Content =
                    spinner = new Spinner {
                    Animate = true,
                    MinHeight = 50,
                    MinWidth = 50
                };
            });

            while (spinner == null)
                await Task.Delay (5);

            _buttonsPerRow = CalculateButtonsPerRow ((float)Size.Width);
            var i = 0;

            _tokenSource = new CancellationTokenSource ();

            try {
                var tiles = await LoadTiles (_tokenSource.Token);

                Application.Invoke (() => {
                    foreach (var tile in tiles) {
                        if (tile == null)
                            continue;

                        if (i >= _buttonsPerRow) {
                            PushNewRow ();
                            i = 0;
                        }

                        try {
                            Xwt.Drawing.Image image = tile.Image?.ToXwtImage ()?.ScaleToSize (100);
                            Widget widget = null;

                            if (ParkitectNexus.Data.Utilities.OperatingSystem.Detect() == SupportedOperatingSystem.Linux){
                                if (image != null) {
                                    Xwt.Drawing.Image lighterImage = image.WithAlpha(.7f);
                                    var clickableImage = new ImageView (image);
                                    clickableImage.ButtonPressed += (sender, args) => tile.ClickAction ();
                                   
                                    clickableImage.MouseEntered += (object sender, EventArgs e) => {
                                        ((ImageView)sender).Image = lighterImage;
                                        ((Widget)sender).BackgroundColor = Color.FromBytes(255,255,255);
                                    };
                                    clickableImage.MouseExited += (object sender, EventArgs e) => {
                                        ((ImageView)sender).Image = image;
                                        ((Widget)sender).BackgroundColor = tile.BackgroundColor;
                                    };

                                    widget = clickableImage;
                                }
                                else
                                {
                                    var label = new Label(tile.Text);
                                    label.ButtonPressed += (sender, args) => tile.ClickAction ();
                                    label.Wrap = WrapMode.Word;
                                    label.TextAlignment = Alignment.Center;
                                   
                                    label.MouseEntered += (object sender, EventArgs e) => {
                                        ((Widget)sender).BackgroundColor = Color.FromBytes(255,255,255);
                                    };
                                    label.MouseExited += (object sender, EventArgs e) => {
                                        ((Widget)sender).BackgroundColor = tile.BackgroundColor;
                                    };

                                    widget = label;

                                }

                            }
                            else
                            {
                                var button = new Button (image) {
                                    Label = tile.Image == null ? tile.Text : null,
                                    TooltipText = tile.Text,
                                    Style = ButtonStyle.Borderless,
                                    BackgroundColor = tile.BackgroundColor,
                                    ImagePosition = ContentPosition.Center
                                };
                                widget = button;
                            }

                            widget.WidthRequest = 100;
                            widget.HeightRequest = 100;
                            widget.MinWidth = 0;
                            widget.BackgroundColor= tile.BackgroundColor;

                            _buttons.Add (widget);
                            _rows.Peek ().PackStart (widget);
                            i++;
                        } catch (Exception e) {
                            _log.WriteLine ("Failed to convert Tile object to UI");
                            _log.WriteException (e);
                        }
                    }

                    Content = _box;
                });
            } catch (TaskCanceledException) {
            } catch (OperationCanceledException) {
            } finally {
                _tokenSource?.Dispose ();
                _tokenSource = null;
            }
        }

        public void HandleSizeUpdate (float width)
        {
            if (CalculateButtonsPerRow (width) == _buttonsPerRow)
                return;

            _buttonsPerRow = CalculateButtonsPerRow (width);
            var i = 0;

            ClearTiles ();
            PushNewRow ();

            foreach (var button in _buttons) {
                if (i >= _buttonsPerRow) {
                    PushNewRow ();
                    i = 0;
                }
                _rows.Peek ().PackStart (button);
                i++;
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
        protected override void OnBoundsChanged ()
        {
            base.OnBoundsChanged ();

            HandleSizeUpdate ((float)Size.Width);
        }

        #endregion

        #region Implementation of IPageView

        public string DisplayName { get; }

        public event EventHandler DisplayNameChanged;

        #endregion
    }
}