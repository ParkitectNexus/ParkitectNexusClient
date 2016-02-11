// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.IO;
using ParkitectNexus.Data.Caching;

namespace ParkitectNexus.Data.Authentication
{
    public class AvatarCache
    {
        public bool HasAvatar { get; set; }

        public ICachedFile File { get; set; } = new CachedFile();

        public Image GetImage()
        {
            using (var stream = File.Open(FileMode.Open))
                return stream == null ? null : Image.FromStream(stream);
        }
    }
}
