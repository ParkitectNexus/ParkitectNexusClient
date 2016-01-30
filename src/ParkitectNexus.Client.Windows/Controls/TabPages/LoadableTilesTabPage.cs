// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public abstract class LoadableTilesTabPage : MetroTabPage, IPresenter
    {
        private readonly MetroProgressSpinner _progressSpinner = new MetroProgressSpinner
        {
            Minimum = 0,
            Maximum = 100,
            Size = new Size(75, 75)
        };

        private bool _entered;
        private bool _loaded;
        private CancellationTokenSource _tokenSource;

        protected LoadableTilesTabPage()
        {
            AutoScroll = true;
        }

        protected abstract bool ReloadOnEnter { get; }

        public void WasSelected()
        {
            RefreshTiles();
        }

        protected abstract Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken);

        protected virtual void ClearTiles()
        {
            foreach (var tile in Controls.OfType<MetroTile>().ToArray())
                Controls.Remove(tile);
        }

        protected void UpdateLoadingProgress(int percentage)
        {
            var value = Math.Min(100, Math.Max(0, percentage));

            if (_progressSpinner.InvokeRequired)
                _progressSpinner.Invoke((MethodInvoker) (() =>
                {
                    if (!_progressSpinner.Disposing || _progressSpinner.IsDisposed)
                        _progressSpinner.Value = value;
                }));
            else if (!_progressSpinner.Disposing || _progressSpinner.IsDisposed)
                _progressSpinner.Value = value;
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

            // Add load spinner
            UpdateLoadingProgress(0);
            _progressSpinner.Location = new Point(Size.Width/2 - _progressSpinner.Width/2,
                Size.Height/2 - _progressSpinner.Height/2);
            Controls.Add(_progressSpinner);

            const int marginX = 5;
            const int marginY = 5;
            var size = new Size(100, 100);
            var countX = (Width - marginX - VerticalScrollbarSize)/(size.Width + marginX);

            if (countX < 1) countX = 1;

            var i = 0;

            _tokenSource = new CancellationTokenSource();

            try
            {
                var tiles = await LoadTiles(_tokenSource.Token);

                foreach (var tile in tiles)
                {
                    if (tile == null) continue;

                    var x = i%countX;
                    var y = i/countX;
                    tile.Location = new Point(Margin.Left + (x*(size.Width + marginX)),
                        Margin.Top + (y*(size.Height + marginY)));
                    tile.Size = size;

                    Controls.Add(tile);
                    i++;
                }
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

            Controls.Remove(_progressSpinner);
        }

        #region Overrides of TabPage

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event of the
        ///     <see cref="T:System.Windows.Forms.TabPage" />.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnEnter(EventArgs e)
        {
            if (!_entered)
            {
                _entered = true;
                //if (ReloadOnEnter)
                //     RefreshTiles();
            }

            base.OnEnter(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event of the
        ///     <see cref="T:System.Windows.Forms.TabPage" />.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLeave(EventArgs e)
        {
            _entered = false;
            base.OnLeave(e);
        }

        #endregion

        #region Overrides of Panel

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnParentChanged(EventArgs e)
        {
            if (!ReloadOnEnter && !_loaded)
                RefreshTiles();

            _loaded = true;

            base.OnParentChanged(e);
        }

        /// <summary>
        ///     Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of
        ///     actually listening to the event, but should still call base.onResize to ensure that the event is fired for external
        ///     listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnResize(EventArgs eventargs)
        {
            const int marginX = 5;
            const int marginY = 5;
            var size = new Size(100, 100);
            var countX = (Width - marginX - VerticalScrollbarSize)/(size.Width + marginX);

            if (countX > 0)
            {
                var i = 0;
                foreach (var tile in Controls.OfType<MetroTile>())
                {
                    var x = i%countX;
                    var y = i/countX;
                    tile.Location = new Point(Margin.Left + (x*(size.Width + marginX)),
                        Margin.Top + (y*(size.Height + marginY)));
                    i++;
                }
            }

            base.OnResize(eventargs);
        }

        #endregion
    }
}