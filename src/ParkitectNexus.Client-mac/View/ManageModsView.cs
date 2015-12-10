using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Clientmac;
using System.Collections.Generic;
using MonoMac.Foundation;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.MacOSX;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ParkitectNexus.Client.View
{
    public class ManageModsView : BaseView
    {
        private NSTextField _label1;
        private NSButton _backButton;

		private IParkitect _parkitect;

		IParkitectMod _selectedMod;

		/**
		 * Right side fields
		 **/
		NSTextField _modName;
		NSTextField _version;
		NSTextField _inDevelopment;
		NSButton _checkForUpdates;
		NSButton _uninstall;
		NSButton _website;

        public ManageModsView(IParkitect parkitect)
        {
			_parkitect = parkitect;

            _label1 = new NSTextField(new Rectangle(10, 275, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Select a mod to disable, update or uninstall it.",
                Font = NSFont.SystemFontOfSize(10)
            };

			int height = 270;

			foreach(IParkitectMod mod in _parkitect.InstalledMods)
			{
				NSButton modButton = new NSButton (new Rectangle(10, height -= 20, 150, 15)) {
					AutoresizingMask = NSViewResizingMask.MinYMargin,
					BezelStyle = NSBezelStyle.Rounded,
					Title = mod.Name,
					Alignment = NSTextAlignment.Left,
					Bordered = false
				};

				AddSubview (modButton);

				modButton.Activated += (sender, e) => {
					_selectedMod = mod;

					UpdateModInfoView();
				};
			}

            _backButton = new NSButton(new Rectangle(399, 10, 81, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "< Back",
                Bordered = true,
            };

			AddSubview(_label1);
            AddSubview(_backButton);

			InitializeModView ();

            AwakeFromNib();
        }

        public static object vw;
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _backButton.Activated += (sender, e) =>
            {
                Window.ContentView.ReplaceSubviewWith(this, new MainMenuView());
            };
        }

		private void InitializeModView()
		{
			NSTextField modNameLabel = new NSTextField(new Rectangle(200, 250, 300, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "Mod: ",
				Font = NSFont.SystemFontOfSize(10)
			};

			_modName = new NSTextField(new Rectangle(230, 250, 300, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "",
				Font = NSFont.SystemFontOfSize(10)
			};

			NSTextField versionLabel = new NSTextField(new Rectangle(200, 230, 300, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "Version: ",
				Font = NSFont.SystemFontOfSize(10)
			};

			_version = new NSTextField(new Rectangle(250, 230, 300, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "",
				Font = NSFont.SystemFontOfSize(10)
			};

			NSTextField websiteLabel = new NSTextField(new Rectangle(200, 210, 280, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "Website: ",
				Font = NSFont.SystemFontOfSize(10)
			};

			_website = new NSButton (new Rectangle (250, 212, 170, 20)) {
				AutoresizingMask = NSViewResizingMask.MinYMargin,
				BezelStyle = NSBezelStyle.Rounded,
				Title = "View on ParkitectNexus",
				Bordered = true,
			};

			_inDevelopment = new NSTextField(new Rectangle(200, 160, 280, 20)) {
				BackgroundColor = NSColor.Clear,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "MOD IS IN DEVELOPMENT",
				Font = NSFont.SystemFontOfSize(10)
			};

			_checkForUpdates = new NSButton(new Rectangle(200, 130, 280, 20)) {
				AutoresizingMask = NSViewResizingMask.MinYMargin,
				BezelStyle = NSBezelStyle.Rounded,
				Title = "Check for updates",
				Bordered = true,
			};

			_uninstall = new NSButton(new Rectangle(200, 100, 280, 20)) {
				AutoresizingMask = NSViewResizingMask.MinYMargin,
				BezelStyle = NSBezelStyle.Rounded,
				Title = "Uninstall",
				Bordered = true,
			};

			_website.Activated += (sender, e) => {
				Process.Start($"https://client.parkitectnexus.com/redirect/{_selectedMod.Repository}");
			};

			_checkForUpdates.Activated += (sender, e) => {
				// todo yo
			};

			_uninstall.Activated += (sender, e) => {
				_selectedMod.Delete();

				Window.ContentView.ReplaceSubviewWith(this, new ManageModsView(_parkitect));
			};

			AddSubview(modNameLabel);
			AddSubview(_modName);

			AddSubview (versionLabel);
			AddSubview (_version);

			AddSubview (websiteLabel);
			AddSubview (_website);

			AddSubview (_inDevelopment);
			AddSubview (_checkForUpdates);
			AddSubview (_uninstall);
		}

		private void UpdateModInfoView()
		{
			_modName.StringValue = _selectedMod.Name;
			_version.StringValue = !string.IsNullOrEmpty (_selectedMod.Tag) ? _selectedMod.Tag : "";
			_inDevelopment.StringValue = _selectedMod.IsDevelopment ? "Mod is in development" : "";
			_website.Enabled = !_selectedMod.IsDevelopment;
		}
    }
}

