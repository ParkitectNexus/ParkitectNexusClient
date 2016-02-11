// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using Newtonsoft.Json;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var registry = ObjectFactory.ConfigureStructureMap();
            ObjectFactory.SetUpContainer(registry);

            using (var wc = new NexusWebClientFactory().CreateWebClient())
            {
                var deserialized = JsonConvert.DeserializeObject<ApiDataContainer<ApiAsset>>
                    (wc.DownloadString("http://staging.parkitectnexus.com/api/assets/12124527df"));
                Console.WriteLine(deserialized);
            }

            Console.ReadLine();
        }
    }
}