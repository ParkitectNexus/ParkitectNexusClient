// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace ParkitectNexus.Data.Utilities
{
    public static class ImageUtility
    {
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            if (image == null)
                return null;
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