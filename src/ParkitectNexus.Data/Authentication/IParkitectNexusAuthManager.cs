using System.Drawing;
using System.Threading.Tasks;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Authentication
{
    public interface IParkitectNexusAuthManager
    {
        bool IsAuthenticated { get; }
        string Key { get; set; }
        Task<ApiUser> GetUser();
        Task<ApiSubscription[]> GetSubscriptions();
        Task<ApiAsset[]> GetSubscribedAssets();
        void OpenLoginPage();
        void ReloadKey();
        void Logout();
        Task<Image> GetAvatar();
    }
}
