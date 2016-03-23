using System;
using MonoMac.AppKit;
using System.Drawing;
using System.IO;

namespace ParkitectNexus.MacOSX
{
    public static class NSImageUtility
    {
        public static NSImage ToNSImage(this Image img) {
            if(img == null)
                return null;
            
            using(var s = new MemoryStream())
            {
                img.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, System.IO.SeekOrigin.Begin);
                return NSImage.FromStream(s);
            }
        }
    }
}

