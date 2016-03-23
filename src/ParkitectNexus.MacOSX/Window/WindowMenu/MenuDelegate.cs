using System;
using MonoMac.AppKit;
using System.Drawing;
using MonoMac.Foundation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.MacOSX
{

    public class MenuDelegate : NSOutlineViewDelegate
    {
        NSOutlineView _menuView;
        MainWindow _window;
        public MenuDelegate(MainWindow window, NSOutlineView menuView)
        {
            _menuView = menuView;
            _window = window;
        }

        public override bool ShouldEditTableColumn(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            return false;
        }

        public override void SelectionDidChange(NSNotification notification)
        {  
            switch(_menuView.SelectedRow)
            {
            case 0:
                // Main menu
                _window.SetView<MainMenuView>();
                break;
            case 1:
                // Mods
                _window.SetView<ModsView>();
                break;
            }
        }
    }
    
}
