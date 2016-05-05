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

using System.IO;
using System.Security.Cryptography;

namespace ParkitectNexus.Data.Utilities
{
    public static class StreamUtility
    {
        public static byte[] CreateMD5Checksum(this Stream stream, bool startAtBeginning = true, MD5 md5 = null)
        {
            if (md5 == null)
                md5 = MD5.Create();

            if (startAtBeginning)
                stream.Seek(0, SeekOrigin.Begin);

            return md5.ComputeHash(stream);
        }
    }
}