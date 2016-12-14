using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Base.Pages
{
    public class ScenariosPageView : AssetsPageView
    {
        public ScenariosPageView(IParkitect parkitect, IWebsite website, ILogger log, IPresenter parent)
            : base(parkitect, website, log, AssetType.Scenario, parent, "Scenarios")
        {
        }
    }
}
