using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Test.Web
{
    [TestClass]
    public class ParkitectNexusUrlTest
    {
        [TestMethod]
        public void TryParseTestMod()
        {
            //arrange
            string mod1 = "parkitectnexus://Ambient+Occlusion/mod/ParkitectNexus%2FAmbientOcclusion";
            string mod2 = "parkitectnexus://Cheat+mod/mod/nozols%2FParkitectCheats";

            //act

            var result1 = ParkitectNexusUrl.Parse(mod1);
            var result2 = ParkitectNexusUrl.Parse(mod2);

            //assert
            Assert.IsTrue(result1.Name == "Ambient Occlusion");
            Assert.IsTrue(result1.FileHash == "ParkitectNexus/AmbientOcclusion");
            Assert.IsTrue(result1.AssetType == ParkitectAssetType.Mod);

            Assert.IsTrue(result2.Name == "Cheat mod");
            Assert.IsTrue(result2.FileHash == "nozols/ParkitectCheats");
            Assert.IsTrue(result2.AssetType == ParkitectAssetType.Mod);
        }

        [TestMethod]
        public void TryParseBlueprint()
        {
            //arrange
            string blueprint1 = "parkitectnexus://Dropper/blueprint/3ed8ad1d70";
            string blueprint2 = "parkitectnexus://Ski+Racer/blueprint/2cfc80d929";

            //act

            var result1 = ParkitectNexusUrl.Parse(blueprint1);
            var result2 = ParkitectNexusUrl.Parse(blueprint2);

            //assert
            Assert.IsTrue(result1.Name == "Dropper");
            Assert.IsTrue(result1.FileHash == "3ed8ad1d70");
            Assert.IsTrue(result1.AssetType == ParkitectAssetType.Blueprint);

            Assert.IsTrue(result2.Name == "Ski Racer");
            Assert.IsTrue(result2.FileHash == "2cfc80d929");
            Assert.IsTrue(result2.AssetType == ParkitectAssetType.Blueprint);
        }

        [TestMethod]
        public void TryParseSaveGame()
        {
            //arrange
            string saveGame1 = "parkitectnexus://Gigantic+multi-themed+park./savegame/a4a24d1acf";
            string saveGame2 = "parkitectnexus://Log+Hill/savegame/03567c46bb";

            //act

            var result1 = ParkitectNexusUrl.Parse(saveGame1);
            var result2 = ParkitectNexusUrl.Parse(saveGame2);

            //assert
            Assert.IsTrue(result1.Name == "Gigantic multi-themed park.");
            Assert.IsTrue(result1.FileHash == "a4a24d1acf");
            Assert.IsTrue(result1.AssetType == ParkitectAssetType.Savegame);

            Assert.IsTrue(result2.Name == "Log Hill");
            Assert.IsTrue(result2.FileHash == "03567c46bb");
            Assert.IsTrue(result2.AssetType == ParkitectAssetType.Savegame);
        }
    }
}