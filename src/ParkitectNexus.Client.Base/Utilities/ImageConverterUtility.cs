// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;
using Xwt.Drawing;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace ParkitectNexus.Client.Base.Utilities
{
    public static class ImageConverterUtility
    {
        public static Image ToXwtImage(this System.Drawing.Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return Image.FromStream(memoryStream);
            }
        }
    }
}
