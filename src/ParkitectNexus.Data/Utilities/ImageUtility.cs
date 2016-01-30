﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.Drawing.Drawing2D;

namespace ParkitectNexus.Data.Utilities
{
    public static class ImageUtility
    {
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var result = new Bitmap(width, height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }

        public static Bitmap RecolorImage(Image input, Color color)
        {
            var image = (Bitmap) input;

            for (var y = 0; y < image.Height; y++)
                for (var x = 0; x < image.Height; x++)
                {
                    var currentColor = image.GetPixel(x, y);
                    image.SetPixel(x, y, Color.FromArgb(currentColor.A, color));

                }

            return image;
        }
    }
}
