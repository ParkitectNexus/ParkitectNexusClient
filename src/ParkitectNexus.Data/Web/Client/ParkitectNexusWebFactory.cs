// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.Client
{
    public class ParkitectNexusWebFactory : IParkitectNexusWebFactory
    {
        public IParkitectNexusWebClient CreateWebClient()
        {
            return ObjectFactory.Container.GetInstance<IParkitectNexusWebClient>();
        }
    }
}