// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.Client
{
    public class ParkitectNexusWebClientFactory : IParkitectNexusWebClientFactory
    {
        public IParkitectNexusWebClient CreateWebClient()
        {
            return ObjectFactory.GetInstance<IParkitectNexusWebClient>();
        }
    }
}
