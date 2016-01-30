// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.Client
{
    public class ParkitectNexusWebClientFactory : IParkitectNexusWebClientFactory
    {
        public IParkitectNexusWebClient CreateWebClient()
        {
            return CreateWebClient(false);
        }

        public IParkitectNexusWebClient CreateWebClient(bool authorize)
        {
            var client = ObjectFactory.GetInstance<IParkitectNexusWebClient>();

            if (authorize)
                client.Authorize();

            return client;
        }
    }
}
