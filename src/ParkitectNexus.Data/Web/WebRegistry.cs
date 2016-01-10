using Octokit;
using ParkitectNexus.Data.Web.Client;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IParkitectNexusWeb>().Use<ParkitectNexusWeb>();
            For<IParkitectNexusWebFactory>().Use<ParkitectNexusWebFactory>().Singleton();

            For<IParkitectNexusWebsite>().Use<ParkitectNexusWebsite>();
            For<IParkitectOnlineAssetRepository>().Use<ParkitectOnlineAssetRepository>();

            For<IGitHubClient>().Use(() => new GitHubClient( new ProductHeaderValue("parkitect-nexus-client")));//().SelectConstructor(() => new GitHubClient())

            //ForConcreteType<GitHubClient>().Configure.Ctor<ProductHeaderValue>("productInformation").Is(new ProductHeaderValue("parkitect-nexus-client"));

        }
    }
}
