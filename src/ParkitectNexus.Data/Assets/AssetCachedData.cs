// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.IO;
using ParkitectNexus.Data.Caching;

namespace ParkitectNexus.Data.Assets
{
    public class AssetCachedData
    {
        public string Name { get; set; }
        public ICachedFile ThumbnailFile { get; set; } = new CachedFile();
        public ICachedFile ImageFile { get; set; } = new CachedFile();

        public Image GetThumbnail()
        {
            using (var stream = ThumbnailFile.Open(FileMode.Open))
                return stream == null ? null : Image.FromStream(stream);
        }

        public Image GetImage()
        {
            using (var stream = ImageFile.Open(FileMode.Open))
                return stream == null ? null : Image.FromStream(stream);
        }
    }
}
