// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.Client
{
    public class NexusWebClientFactory : INexusWebClientFactory
    {
        public INexusWebClient CreateWebClient()
        {
            return CreateWebClient(false);
        }

        public INexusWebClient CreateWebClient(bool authorize)
        {
            var client = ObjectFactory.GetInstance<INexusWebClient>();

            if (authorize)
                client.Authorize();

            return client;
        }
    }
}