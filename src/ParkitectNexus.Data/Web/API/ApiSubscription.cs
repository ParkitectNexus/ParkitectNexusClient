// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    [JsonObject]
    public class ApiSubscription
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("subscribable_id")]
        public int SubscribableId { get; set; }

        [JsonProperty("subscribable_type")]
        public string SubscribableType { get; set; }

        [JsonProperty("subscribable")]
        public ApiResourcePromiseWithUrl<ApiAsset> Subscribable { get; set; }

        public async Task<ApiAsset> GetAsset()
        {
            switch (SubscribableType)
            {
			case "asset":
				if (Subscribable == null)
					return null;
                    return await Subscribable.GetResource();
                default:
                    return null;
            }
        }
    }
}