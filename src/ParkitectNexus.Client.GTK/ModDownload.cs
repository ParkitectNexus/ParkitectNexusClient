using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using Gtk;
using System.Threading;
using ParkitectNexus.Data.Utilities;
using System.Linq;

namespace ParkitectNexus.Client.GTK
{
	public partial class ModDownload : Gtk.Dialog
	{
		private readonly IParkitect _parkitect;
		private readonly ParkitectNexusUrl _parkitectNexusUrl;
		private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
		private int _dots;
		private int _dotsDirection = 1;
		private string _keyword = "Downloading";

		private volatile bool isFinished = false;

		public static bool Download(ParkitectNexusUrl parkitectNexusUrl, IParkitect parkitect,
			IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
		{
			// Run the download process in an installer form, for a nice visible process.
			var form = new ModDownload(parkitect,parkitectOnlineAssetRepository,parkitectNexusUrl);

			switch (form.Run ()) {
			case (int)Gtk.ResponseType.Apply:
				form.Destroy ();
				return true;
			default:
				return false;
			}

		}

		public static bool Download(string url, IParkitect parkitect,
			IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
		{
			// Try to parse the specified download url. If parsing fails open ParkitectNexus. 
			ParkitectNexusUrl parkitectNexusUrl;
			if (!ParkitectNexusUrl.TryParse(url, out parkitectNexusUrl))
			{
				// Create a form to allow the dialogs to have a owner with forced focus and an icon.
				Gtk.MessageDialog errorDialog = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok,"The URL you opened is invalid!");
				errorDialog.Run ();
				errorDialog.Destroy();
				return false;
			}
			return Download (parkitectNexusUrl, parkitect, parkitectOnlineAssetRepository);
	
		}

		public ModDownload (IParkitect parkitect, IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, ParkitectNexusUrl parkitectNexusUrl)
		{
			if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
			if (parkitectOnlineAssetRepository == null)
				throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
			if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
			_parkitect = parkitect;
			_parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
			_parkitectNexusUrl = parkitectNexusUrl;

			this.Build ();
		
			// Format the "installing" label.
			//installingLabel.Text = "Please wait while ParkitectNexus is installing {parkitectNexusUrl.AssetType} \"{parkitectNexusUrl.Name}\".";
			this.lblModName.Text = "Please wait while ParkitectNexus is installing "+_parkitectNexusUrl.AssetType+" \""+_parkitectNexusUrl.Name+"\".";

			GLib.Timeout.Add (100, new GLib.TimeoutHandler (UpdateProgress));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DownloadLabelUpdate));

		
			Thread download = new Thread (new ThreadStart (Process));
			download.Start ();
		}

		/// <summary>
		/// update the download label
		/// </summary>
		private bool DownloadLabelUpdate()
		{
			if (isFinished == true) {
				lblProgressLabel.Text = "Done!";
				return false;
			} else {
				//Downloading... Effect. 

				// Add or subtract dots.
				_dots += _dotsDirection;

				// If the dots count is out of the boundaries, flip the dots direction.
				if (_dots <= 0 || _dots > 4)
					_dotsDirection = -_dotsDirection;

				// Update the status label.
				lblProgressLabel.Text = _keyword + "." + string.Concat (Enumerable.Repeat (".", _dots));
			}
			return true;
		}

		/// <summary>
		/// update the loading bar
		/// </summary>
		private bool UpdateProgress()
		{
			if (isFinished == true) {
				if (!btnFinished.Sensitive) {
					installProgress.Activate ();
					btnFinished.Sensitive = true;
				}
				installProgress.Fraction += 0.1;
				if (installProgress.Fraction >= 1)
					return false;
			} else {
				installProgress.Pulse ();

			}
			return true;
		}

		/// <summary>
		/// Process and install the mods
		/// </summary>
		private async void Process()
		{

			var assetName = _parkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name;
			try
			{
				// Download the asset.
				var asset = await _parkitectOnlineAssetRepository.DownloadFile(_parkitectNexusUrl);

				if (asset == null)
				{
					Gtk.Application.Invoke (delegate {
						Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, $"Failed to install {assetName}!\nPlease try again later.","ParkitectNexus Client");
						errorDialog.Run ();	
						this.Destroy();
						this.Respond(ResponseType.Close);

					});
					return;
						
				}

				_keyword = "Installing";

				await _parkitect.StoreAsset(asset);

				asset.Dispose();
			}
			catch (Exception e)
			{
				Log.WriteLine($"Failed to install {assetName}!");
				Log.WriteException(e);

				// If the asset has failed to download, show some feedback to the user.
				Gtk.Application.Invoke (delegate {
					Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, $"Failed to install {assetName}!\nPlease try again later.\n\n{e.Message}");
					errorDialog.Run ();	
					this.Destroy();
					this.Respond(ResponseType.Close);

				});

			}
			finally
			{
				isFinished = true;
	
			}
		
		}


	}
}

