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
            var lastSubView = splitView.Subviews.Last();
            firstSubView.Frame = new RectangleF(0, 0, 200, newSize.Height);

            if(lastSubView == firstSubView)
                return;

            lastSubView.Frame = new RectangleF(200, 0, newSize.Width - 200, newSize.Height);
        }
    }
    
}
