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

using System;
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

        public static Image ScaleToSize(this Image image, int size)
        {
            if (image == null)
                return null;

            var oldSize = Math.Max(image.Width, image.Height);

            return size > oldSize
                ? image
                : image.Scale(size/oldSize);
        }
    }
}
