// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Web.API
{
    [JsonObject]
    public class ApiSubscription
    {
        public int Id { get; set; }
        public int SubscribableId { get; set; }
        public ParkitectAssetType SubscribableType { get; set; }
        public string ApiUrl { get; set; }

        public async Task<IApiResource> GetResource()
        {
            switch (SubscribableType)
            {
                case ParkitectAssetType.Blueprint:
                    return await new ApiResourcePromise<ApiBlueprintResource>
                    {
                        ApiUrl = ApiUrl
                    }.GetResource();
                case ParkitectAssetType.Mod:
                    return await new ApiResourcePromise<ApiModResource>
                    {
                        ApiUrl = ApiUrl
                    }.GetResource();
                default:
                    return null;
            }
        }
    }
}
