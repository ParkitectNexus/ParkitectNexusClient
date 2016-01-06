using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Octokit;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Test.Web
{
    [TestClass]
    public class ParkitectOnlineAssetRepositoryTest
    {
        [TestMethod]
        public async Task ResolveDownloadInfoBlueprintTest()
        {
            //arrange
            var assetRepository = new Mock<ParkitectOnlineAssetRepository>(new ParkitectNexusWebsite());
            assetRepository.As<IParkitectOnlineAssetRepository>();
            assetRepository.Protected()
                .Setup<Task<RepositoryTag>>("GetLatestModTag", ItExpr.IsNull<string>(), ItExpr.IsNull<GitHubClient>())
                .Returns(
                    new Task<RepositoryTag>(
                        () => new RepositoryTag("test_name", new GitReference(), "zip_url", "tar_ball_url")));

            ParkitectOnlineAssetRepository onlineAssetRepository = assetRepository.Object;

            var parkitectNexusURL = new Mock<IParkitectNexusUrl>();

            //parkitectnexus://Basic+Long+Wooden+Coaster/blueprint/27cff6bd3e
            parkitectNexusURL.Setup(x => x.FileHash).Returns("27cff6bd3e");
            parkitectNexusURL.Setup(x => x.Name).Returns("Basic+Long+Wooden+Coaster");
            parkitectNexusURL.Setup(x => x.AssetType).Returns(ParkitectAssetType.Blueprint);

            //act
            var result = await onlineAssetRepository.ResolveDownloadInfo(parkitectNexusURL.Object);


            //assert
            assetRepository.Protected()
                .Verify("GetLatestModTag", Times.Never(), ItExpr.IsAny<string>(), ItExpr.IsAny<GitHubClient>());
            Assert.IsTrue(result.Url == "http://staging.parkitectnexus.com/download/27cff6bd3e" ||
                          result.Url == "http://parkitectnexus.com/download/27cff6bd3e");
        }

        [TestMethod]
        public async Task ResolveDownloadInfoDownloadSavegameTest()
        {
            //arrange
            var assetRepository = new Mock<ParkitectOnlineAssetRepository>(new ParkitectNexusWebsite());
            assetRepository.As<IParkitectOnlineAssetRepository>();
            assetRepository.Protected()
                .Setup<Task<RepositoryTag>>("GetLatestModTag", ItExpr.IsNull<string>(), ItExpr.IsNull<GitHubClient>())
                .Returns(
                    new Task<RepositoryTag>(
                        () => new RepositoryTag("test_name", new GitReference(), "zip_url", "tar_ball_url")));

            ParkitectOnlineAssetRepository onlineAssetRepository = assetRepository.Object;

            var parkitectNexusURL = new Mock<IParkitectNexusUrl>();

            //parkitectnexus://Basic+Long+Wooden+Coaster/blueprint/27cff6bd3e
            parkitectNexusURL.Setup(x => x.FileHash).Returns("27cff6bd3e");
            parkitectNexusURL.Setup(x => x.Name).Returns("Basic+Long+Wooden+Coaster");
            parkitectNexusURL.Setup(x => x.AssetType).Returns(ParkitectAssetType.Blueprint);

            //act
            var result = await onlineAssetRepository.ResolveDownloadInfo(parkitectNexusURL.Object);


            //assert
            assetRepository.Protected()
                .Verify("GetLatestModTag", Times.Never(), ItExpr.IsAny<string>(), ItExpr.IsAny<GitHubClient>());
            Assert.IsTrue(result.Url == "http://staging.parkitectnexus.com/download/27cff6bd3e" ||
                          result.Url == "http://parkitectnexus.com/download/27cff6bd3e");
        }

        [TestMethod]
        public async Task ResolveDownloadInfoModTest()
        {
            //arrange
            var assetRepository = new Mock<ParkitectOnlineAssetRepository>(new ParkitectNexusWebsite());
            assetRepository.As<IParkitectOnlineAssetRepository>();
            assetRepository.Protected()
                .Setup<Task<RepositoryTag>>("GetLatestModTag", ItExpr.IsAny<string>(), ItExpr.IsAny<GitHubClient>())
                .ReturnsAsync(new RepositoryTag("test_name", new GitReference(), "zip_url", "tar_ball_url"));

            ParkitectOnlineAssetRepository onlineAssetRepository = assetRepository.Object;

            var parkitectNexusURL = new Mock<IParkitectNexusUrl>();

            //parkitectnexus://Tropical+Trees+Pack/mod/Renaleteto/parkitect-tropical-trees-pack
            parkitectNexusURL.Setup(x => x.FileHash).Returns("Renaleteto/parkitect-tropical-trees-pack");
            parkitectNexusURL.Setup(x => x.Name).Returns("Tropical+Trees+Pack");
            parkitectNexusURL.Setup(x => x.AssetType).Returns(ParkitectAssetType.Mod);

            //act
            var result = await onlineAssetRepository.ResolveDownloadInfo(parkitectNexusURL.Object);

            //assert
            assetRepository.Protected()
                .Verify("GetLatestModTag", Times.Once(),
                    ItExpr.Is<string>(x => x == "Renaleteto/parkitect-tropical-trees-pack"),
                    ItExpr.IsAny<GitHubClient>());
            Assert.IsTrue(result.Url == "zip_url");
            Assert.IsTrue(result.Tag == "test_name");
            Assert.IsTrue(result.Repository == "Renaleteto/parkitect-tropical-trees-pack");
        }

        [TestMethod]
        public void IsValidFileHashTest()
        {
            //assert
            Assert.IsTrue(ParkitectOnlineAssetRepository.IsValidFileHash("27cff6bd3e", ParkitectAssetType.Savegame));
            Assert.IsFalse(ParkitectOnlineAssetRepository.IsValidFileHash("27cfdddf6bd3e", ParkitectAssetType.Savegame));
            Assert.IsFalse(ParkitectOnlineAssetRepository.IsValidFileHash("2$7cff6bd3e", ParkitectAssetType.Savegame));

            Assert.IsTrue(ParkitectOnlineAssetRepository.IsValidFileHash("Renaleteto/parkitect-tropical-trees-pack",
                ParkitectAssetType.Mod));
        }
    }
}