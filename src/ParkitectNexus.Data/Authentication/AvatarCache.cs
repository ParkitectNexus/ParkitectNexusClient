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