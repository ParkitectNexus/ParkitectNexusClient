// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Octokit;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Client;
using StructureMap;

namespace ParkitectNexus.Data.Web
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IParkitectNexusWebClient>().Use<ParkitectNexusWebClient>();
            For<IParkitectNexusWebClientFactory>().Use<ParkitectNexusWebClientFactory>().Singleton();

            For<IParkitectNexusWebsite>().Use<ParkitectNexusWebsite>();
            For<IRemoteAssetRepository>().Use<RemoteAssetRepository>();

            For<IGitHubClient>().Use(() => new GitHubClient(new ProductHeaderValue("parkitect-nexus-client")));

            For<IParkitectNexusAPI>().Use<ParkitectNexusAPI>();
        }
    }
}
