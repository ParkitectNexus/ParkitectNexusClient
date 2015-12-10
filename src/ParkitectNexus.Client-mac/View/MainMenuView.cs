using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.MacOSX;

namespace ParkitectNexus.Client.View
{
    public class MainMenuView : BaseView
    {
        private IParkitect _parkitect;
        private IParkitectNexusWebsite _parkitectNexusWebsite;

        private NSButton _manageModsButton;
        private NSButton _launchParkitectButton;
        private NSButton _visitParkitectNexusButton;
        private NSButton _closeButton;
        private NSTextField _label1;
        private NSTextField _label2;
        private NSTextField _label3;

        public MainMenuView()
        {
            _parkitect = new MacOSXParkitect();
            _parkitectNexusWebsite = new ParkitectNexusWebsite();

            _manageModsButton = new NSButton(new Rectangle(55, 249, 152, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "Manage Mods",
                Bordered = true,
            };
            _launchParkitectButton = new NSButton(new Rectangle(55, 170, 152, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "Launch Parkitect",
                Bordered = true,
            };
            _visitParkitectNexusButton = new NSButton(new Rectangle(55, 90, 152, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "Visit ParkitectNexus",
                Bordered = true,
            };
            _closeButton = new NSButton(new Rectangle(399, 10, 81, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Title = "Close",
                Bordered = true,
            };

            _label1 = new NSTextField(new Rectangle(83, 231, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Disable, update or uninstall your mods.",
                Font = NSFont.SystemFontOfSize(10)
            };
            _label2 = new NSTextField(new Rectangle(83, 148, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Launch Parkitect with all your enabled mods.",
                Font = NSFont.SystemFontOfSize(10)
            };
            _label3 = new NSTextField(new Rectangle(83, 68, 470, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Visit ParkitectNexus.COm to download new mods, blueprints and savegames.",
                Font = NSFont.SystemFontOfSize(10)
            };

            AddSubview(_manageModsButton);
            AddSubview(_launchParkitectButton);
            AddSubview(_visitParkitectNexusButton);
            AddSubview(_closeButton);
            AddSubview(_label1);
            AddSubview(_label2);
            AddSubview(_label3);

            AwakeFromNib();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _manageModsButton.Activated += (sender, e) =>
            {
				Window.ContentView.ReplaceSubviewWith(this, new ManageModsView(_parkitect));
            };
            _launchParkitectButton.Activated += (sender, e) =>
            {
                _parkitect.Launch();
                Window.Terminate();
            };
            _visitParkitectNexusButton.Activated += (sender, e) =>
            {
                _parkitectNexusWebsite.Launch();
                Window.Terminate();
            };
            _closeButton.Activated += (sender, e) =>
            {
                Window.Terminate();
            };
        }

    }
}

