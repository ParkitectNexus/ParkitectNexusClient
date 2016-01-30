using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
