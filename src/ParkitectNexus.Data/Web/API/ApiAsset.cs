// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents an API asset.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiAsset
    {
        [JsonProperty("type")]
        public string TypeString { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public AssetType Type => AssetTypeUtil.Parse(TypeString);

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("identifier")]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the thumbnail.
        /// </summary>
        [JsonProperty("thumbnail")]
        public ApiAlbumImagePromise Thumbnail { get; set; }

        /// <summary>
        ///     Gets or sets the album.
        /// </summary>
        [JsonProperty("album")]
        public IEnumerable<ApiAlbumImagePromise> Album { get; set; }

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        [JsonProperty("tags")]
        public IEnumerable<ApiResourcePromise<ApiTag>> Tags { get; set; }

        /// <summary>
        ///     Gets or sets the user URL.
        /// </summary>
        [JsonProperty("user")]
        public string UserUrl { get; set; }

        /// <summary>
        ///     Gets or sets the resource.
        /// </summary>
        [JsonProperty("resource")]
        public ApiResourcePromise<ApiMixedResource> Resource { get; set; }

        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the download URL.
        /// </summary>
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Gets or sets the 'create at' date.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreateAt { get; set; }

        /// <summary>
        ///     Gets or sets the 'updated at' date.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        public async Task<IApiResource> GetResource()
        {
            var mixed = await Resource.GetResource();

            switch (Type)
            {
                case AssetType.Blueprint:
                    return new ApiBlueprintResource
                    {
                        Id = mixed.Id,
                        FileName = mixed.FileName,
                        Asset = mixed.Asset
                    };
                case AssetType.Mod:
                    return new ApiModResource
                    {
                        Id = mixed.Id,
                        Source = mixed.Source
                    };
                default:
                    return null;
            }
        }

        #region Overrides of Object

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"Asset{Type}({Name}; {Id})";
        }

        #endregion
    }
}