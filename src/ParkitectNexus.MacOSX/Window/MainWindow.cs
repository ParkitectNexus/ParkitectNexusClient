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
    [Register("MainWindow")]
    public class MainWindow : NSWindow, IPresenter
    {
        private IPresenterFactory _presenterFactory;
        private IParkitect _parkitect;

        private BaseView _currentView;
        private NSSplitView _splitView;

        public MainWindow(IPresenterFactory presenterFactory, IParkitect parkitect)
            : base(new Rectangle(0, 0, 800, 600), (NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable), NSBackingStore.Buffered, false)
        {
            _presenterFactory = presenterFactory;
            _parkitect = parkitect;

            Title = "ParkitectNexus Client";

            ContentView = _splitView = new NSSplitView(Frame);
            _splitView.IsVertical = true;
            _splitView.DividerStyle = NSSplitViewDividerStyle.Thin;
            _splitView.Delegate = new MainSplitViewDelegate();

            var scrollView = new NSScrollView();

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
   
            scrollView.DocumentView = menuOutlineView;
            _splitView.AddSubview(scrollView);

            SetView<MainMenuView>();
            _splitView.SetPositionOfDivider(200f, 0);

            Center();

        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if(!_parkitect.DetectInstallationPath())
            {
                var alert = new NSAlert {
                    MessageText = "It appears you haven't installed Parkitect!\nPlease make sure you have install your Parkitect.app in your /Applications folder.",
                    AlertStyle = NSAlertStyle.Informational
                };

                alert.RunSheetModal(this);
                Terminate();
            }
        }

        public void SetView<T>() where T : BaseView
        {
            var presenterFactory = ObjectFactory.GetInstance<PresenterFactory>();
            SetView(presenterFactory.InstantiatePresenter<T>());
        }

        public void SetView<T,T2>(T2 with) where T : BaseView
        {
            var view = ObjectFactory.Container.With(with).GetInstance<T> ();
            SetView(view);
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
}

