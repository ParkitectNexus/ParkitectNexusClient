// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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