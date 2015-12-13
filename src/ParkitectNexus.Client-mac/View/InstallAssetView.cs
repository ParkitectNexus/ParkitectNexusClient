using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.View
{
    public class InstallAssetView : BaseView
    {
        private readonly IParkitect _parkitect;
        private readonly ParkitectNexusUrl _parkitectNexusUrl;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

        private NSTextField _label1;
        private NSTextField _label2;
        private NSTextField _label3;
        private NSButton _closeButton;
        private NSProgressIndicator _progressIndicator;

        BaseView _returnView;

        public InstallAssetView(ParkitectNexusUrl url, BaseView returnView)
        {
            if(url == null)
                throw new ArgumentNullException("url");

            _parkitectNexusUrl = url;
            _returnView = returnView;

            _parkitect = new MacOSXParkitect();
            _parkitectOnlineAssetRepository = new ParkitectOnlineAssetRepository(new ParkitectNexusWebsite());

            //493, 311
            _label1 = new NSTextField(new Rectangle(26, 311-52, 500, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = $"Please wait while ParkitectNexus is installing {url.AssetType} \"{url.Name}\".",
                Font = NSFont.SystemFontOfSize(11)
            };
            _label2 = new NSTextField(new Rectangle(26, 311-80, 50, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = $"Status:",
                Font = NSFont.SystemFontOfSize(11)
            };
            _label3 = new NSTextField(new Rectangle(93, 311-80, 100, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = $"Downloading",
                Font = NSFont.SystemFontOfSize(11)
            };

            _progressIndicator = new NSProgressIndicator(new Rectangle(493/2-32/2, 311-150, 32, 32)) {
                Style = NSProgressIndicatorStyle.Spinning,
                UsesThreadedAnimation = true
            };
            
            _closeButton = new NSButton(new Rectangle(399, 10, 81, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "Close",
                Bordered = true,
                Enabled = false,
            };

            AddSubview(_closeButton);
            AddSubview(_label1);
            AddSubview(_label2);
            AddSubview(_label3);
            AddSubview(_progressIndicator);

            AwakeFromNib();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _progressIndicator.StartAnimation(this);

            Window.DoNotDisturb = true;
            InstallAsset();

            _closeButton.Activated += (sender, e) => {
                if(_returnView == null)
                    Window.Terminate();
                else
                    Window.SetView(_returnView);
            };
        }

        private async void InstallAsset()
        {
            var assetName = _parkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name;
            try
            {
                // Download the asset.
                var asset = await _parkitectOnlineAssetRepository.DownloadFile(_parkitectNexusUrl);

                if (asset == null)
                {
                    var alert = new NSAlert () {
                        AlertStyle = NSAlertStyle.Informational,
                        InformativeText = $"Failed to install {assetName}!\nPlease try again later.",
                        MessageText = "Failure",
                    };
                    alert.BeginSheet (Window);

                    Window.SetView(_returnView);
                    return;
                }

                _label3.StringValue = "Installing";

                await _parkitect.StoreAsset(asset);

                asset.Dispose();
            }
            catch (Exception e)
            {
                Log.WriteLine($"Failed to install {assetName}!");
                Log.WriteException(e);

                // If the asset has failed to download, show some feedback to the user.
                var alert = new NSAlert () {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = $"Failed to install {assetName}!\nPlease try again later.\n\n{e.Message}",
                    MessageText = "Failure",
                };
                alert.BeginSheet (Window);
            }
            finally
            {
                Window.DoNotDisturb = false;

                _label3.StringValue = "Done!";
                _progressIndicator.StopAnimation(this);
                _closeButton.Enabled = true;
            }
        }


    }
}

