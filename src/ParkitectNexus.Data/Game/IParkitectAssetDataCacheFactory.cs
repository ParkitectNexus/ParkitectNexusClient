// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Game
{
    public interface IParkitectAssetDataCacheFactory
    {
        IParkitectAssetDataCache<AssetCacheData> GetBlueprintCache();
        IParkitectAssetDataCache<AssetCacheData> GetSavegameCache();
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class AssetCacheData : IDisposable
    {
        ~AssetCacheData()
        {
            Dispose(false);
        }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string ThumbnailBase64 { get; set; }

        public Image Thumbnail
        {
            get
            {
                try
                {
                    var fileBytes = Convert.FromBase64String(ThumbnailBase64);

                    using (var ms = new MemoryStream(fileBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    ThumbnailBase64 = null;
                    return;
                }

                using (var ms = new MemoryStream())
                {
                    value.Save(ms, ImageFormat.Png);
                    ThumbnailBase64 = Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Thumbnail?.Dispose();
            }
        }
    }
}
