// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexusInstaller
{
    internal class ParkitectAsset : IDisposable
    {
        public ParkitectAsset(string fileName, ParkitectAssetType type, Stream stream)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            FileName = fileName;
            Stream = stream;
            Type = type;
        }

        public string FileName { get; }
        public ParkitectAssetType Type { get; }
        public Stream Stream { get; }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~ParkitectAsset()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream?.Dispose();
            }
        }
    }
}