using System;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Drawing;
using MonoMac.CoreGraphics;
using ParkitectNexus.Data.Assets.Modding;
using System.Threading.Tasks;
using System.Collections.Generic;
using ParkitectNexus.Data.Assets;
using System.Threading;
using ParkitectNexus.Data.Game;
using System.Linq;
using ParkitectNexus.Data.Utilities;
using System.IO;

namespace ParkitectNexus.MacOSX
{
    public abstract class LoadableTilesView : BaseView
    {
        private CancellationTokenSource _tokenSource;
        private bool _didLoad;
        NSScrollView _scrollView;
        NSCollectionView _collectionView;

        public LoadableTilesView()
        {
            _scrollView = new NSScrollView(Frame);
            _collectionView = new NSCollectionView(Frame);

            var cvi = new TileCollectionViewItem();
            _collectionView.ItemPrototype = cvi;

            _scrollView.AutoresizingMask = NSViewResizingMask.MinXMargin | NSViewResizingMask.WidthSizable | NSViewResizingMask.MaxXMargin | NSViewResizingMask.MinYMargin | NSViewResizingMask.HeightSizable | NSViewResizingMask.MaxYMargin;    
            _scrollView.DocumentView = _collectionView;
            AddSubview(_scrollView);
        }

        public override void ViewWillDraw()
        {
            base.ViewWillDraw();

            if(_didLoad)
                return;

            _didLoad = true;

            RefreshTiles();
        }

        protected abstract Task<IEnumerable<TilePresentedObject>> LoadTiles(CancellationToken cancellationToken);

        protected virtual void ClearTiles()
        {
            // throw new NotImplementedException();
        }

        protected void UpdateLoadingProgress(int percentage)
        {
            // var value = Math.Min(100, Math.Max(0, percentage));

            // TODO: Update a progress indicator
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

            _tokenSource = new CancellationTokenSource();

            try
            {
                var tiles = await LoadTiles(_tokenSource.Token);

                _collectionView.Content = tiles.ToArray();
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
    }



}

