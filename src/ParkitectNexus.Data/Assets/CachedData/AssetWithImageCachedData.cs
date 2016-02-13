// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;

namespace ParkitectNexus.Data.Assets.CachedData
{
    public class AssetWithImageCachedData : AssetCachedData
    {
        public string ImageBase64 { get; set; }

        public Image GetImage()
        {
            if (ImageBase64 == null)
                return null;

            using (var stream = new MemoryStream(Convert.FromBase64String(ImageBase64)))
                return Image.FromStream(stream);
        }
    }
}
