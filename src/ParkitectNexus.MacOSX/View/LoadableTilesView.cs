using System;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Drawing;
using MonoMac.CoreGraphics;

namespace ParkitectNexus.MacOSX
{
    public class ModsView : LoadableTilesView
    {
        
    }

    public abstract class LoadableTilesView : BaseView
    {
        NSScrollView _scrollView;
        NSCollectionView _collectionView;

        public LoadableTilesView()
        {
        

            var titles = new NSObject[] { 
                new NSString("Item 1"), 
                new NSString("Item 2"),
                new NSString("Item 3"), 
                new NSString("Item 4 has moar text than any other!!"),
                new NSString("Item 5"), 
                new NSString("Item 6"),
                new NSString("Item 7"), 
                new NSString("Item 8"),
            };

            _scrollView = new NSScrollView(Frame);

            _collectionView = new NSCollectionView(Frame);
            var cvi = new MyCollectionViewItem();
            _collectionView.ItemPrototype = cvi;
            _collectionView.Content = titles;

            _scrollView.AutoresizingMask = NSViewResizingMask.MinXMargin | NSViewResizingMask.WidthSizable | NSViewResizingMask.MaxXMargin | NSViewResizingMask.MinYMargin | NSViewResizingMask.HeightSizable | NSViewResizingMask.MaxYMargin;    
            _scrollView.DocumentView = _collectionView;
            AddSubview(_scrollView);
        }
    }

    public class MyCollectionViewItem : NSCollectionViewItem
    {
        private MyView _view;

        public MyCollectionViewItem() : base()
        {

        }

        public MyCollectionViewItem(IntPtr ptr) : base(ptr)
        {

        }

        public override void LoadView ()
        {
            _view = new MyView();
            View = _view;
        }

        public override NSObject RepresentedObject 
        {
            get { return base.RepresentedObject; }

            set 
            {
                if (value == null)
                {
                    base.RepresentedObject = new NSString(string.Empty);

                    _view.Button.Title = string.Empty;
                }
                else
                {
                    base.RepresentedObject = value;
                    _view.Button.Title = value.ToString();
                }
            }
        }
    }

    public class MyView : NSView
    {
        private NSButton button;

        public MyView() : base(new RectangleF(0,0,110,110))
        {
            button = new NSButton(new RectangleF(5,5,100,100));
            button.SetButtonType(NSButtonType.MomentaryPushIn);
            button.Cell = new MyButtonCell();
            using(var s = System.IO.File.OpenRead(NSBundle.MainBundle.PathForResource("placeholder", "jpg")))
                button.Image = NSImage.FromStream(s);
         
            AddSubview(button);
        }

        public NSButton Button
        {
            get { return button; }  
        }       
    }

    public class MyButtonCell : NSButtonCell
    {
        public MyButtonCell()
        {
            BackgroundColor = NSColor.FromDeviceRgba(0, 174 / 255f, 219 / 255f, 1);
            Bordered = false;
        }

        public override RectangleF TitleRectForBounds(RectangleF theRect)
        {
            RectangleF titleFrame = base.TitleRectForBounds(theRect);
            SizeF titleSize = AttributedStringValue.Size;

            return new RectangleF(titleFrame.X, theRect.Y + (theRect.Height - titleSize.Height) * 0.5f, titleFrame.Width, titleFrame.Height);
        }
    }
}

