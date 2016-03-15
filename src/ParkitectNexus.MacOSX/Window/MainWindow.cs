using System;
using MonoMac.AppKit;
using System.Drawing;
using MonoMac.Foundation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ParkitectNexus.MacOSX
{
    [Register("MainWindow")]
    public class MainWindow : NSWindow
    {
        private BaseView _currentView;
        private NSSplitView _splitView;

        public MainWindow()
            : base(new Rectangle(0, 0, 493, 369), (NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable), NSBackingStore.Buffered, false)
        {
            Title = "ParkitectNexus Client";

            ContentView = _splitView = new NSSplitView();
            _splitView.IsVertical = true;
            _splitView.DividerStyle = NSSplitViewDividerStyle.Thin;
            _splitView.Delegate = new MainSplitViewDelegate();

            var menuOutlineView = new NSOutlineView();
            menuOutlineView.IndentationPerLevel = 16.0f;
            menuOutlineView.IndentationMarkerFollowsCell = true;
            menuOutlineView.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.Regular;
            menuOutlineView.HeaderView = null;
            menuOutlineView.BackgroundColor = NSColor.FromDeviceRgba(225f/255f,228f/255f,232f/255f,1f);
            menuOutlineView.DataSource = new MenuDataSource();
            menuOutlineView.Delegate = new MenuDelegate(this, menuOutlineView);
       
            NSTableColumn tableColumn = new NSTableColumn("Name");
            tableColumn.Editable = false;
            menuOutlineView.AddColumn(tableColumn);
            menuOutlineView.OutlineTableColumn = tableColumn;
   
            _splitView.AddSubview(menuOutlineView);
            SetView(new MainMenuView());
            _splitView.SetPositionOfDivider(200f, 0);

            Center();

        }

        public void SetView(BaseView view)
        {
            if(view == null)
                throw new ArgumentNullException("view");

            if(_currentView == null)
            {
                _splitView.AddSubview(view);
            }
            else
            {
                _splitView.ReplaceSubviewWith(_currentView, view);
            }

            _currentView = view;
        }

        public void Terminate()
        {
            NSApplication.SharedApplication.Terminate(this);
        }
    }

    public class MainSplitViewDelegate : NSSplitViewDelegate
    {
        public override RectangleF GetEffectiveRect(NSSplitView splitView, RectangleF proposedEffectiveRect, RectangleF drawnRect, int dividerIndex)
        {
            return new RectangleF();
        }

        public override void Resize(NSSplitView splitView, SizeF oldSize)
        {
            RectangleF newSize = splitView.Frame;

            if(!splitView.Subviews.Any())
                return;

            var firstSubView = splitView.Subviews.First();
            firstSubView.Frame = new RectangleF(firstSubView.Frame.X, firstSubView.Frame.Y, 200, newSize.Height);
        }
    }

    public class MenuDelegate : NSOutlineViewDelegate
    {
        NSOutlineView _menuView;
//        MainWindow _window;
        public MenuDelegate(MainWindow window, NSOutlineView menuView)
        {
            _menuView = menuView;
//            _window = window;
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
                break;
            case 1:
                // Mods
                break;
            }
        }
    }

    public class MenuItem:NSObject
    {
        public string Name { get;set; }
    }

    public class MenuDataSource:NSOutlineViewDataSource
    {
        public List<MenuItem> Items {
            get;
            set;
        }

        public MenuDataSource()
        {
            Items = new List<MenuItem>();
            Items.Add(new MenuItem(){ Name = "Main Menu" });
            Items.Add(new MenuItem(){ Name = "Mods" });
            Items.Add(new MenuItem(){ Name = "Blueprints" });
            Items.Add(new MenuItem(){ Name = "Savegames" });
            Items.Add(new MenuItem(){ Name = "Queue" });
        }

        public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
        {
            return item == null ? Items.Count : 0;
        }

        public override NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn forTableColumn, NSObject byItem)
        {
            return (NSString)((byItem as MenuItem)?.Name ?? string.Empty);
        }

        public override NSObject GetChild (NSOutlineView outlineView, int childIndex, NSObject ofItem)
        {
            return ofItem == null ? Items[childIndex] : null;
        }

        public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
        {
            return false;
        }
    }
}

