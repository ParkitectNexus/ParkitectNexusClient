// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Test.Web
{
    [TestClass]
    public class ParkitectNexusUrlTest
    {
        [TestMethod]
        public void TryParseInstallTest()
        {
            // arrange
            string url1 = "parkitectnexus://install/testidentifier";

            // act
            var result1 = ParkitectNexusUrl.Parse(url1);

            Assert.AreEqual(ParkitectNexusUrlAction.Install, result1.Action);
            Assert.IsInstanceOfType(result1.Data, typeof(ParkitectNexusInstallUrlAction));
            Assert.AreEqual("testidentifier", (result1.Data as ParkitectNexusInstallUrlAction)?.Id);
        }
        [TestMethod]
        public void TryParseAuthTest()
        {
            // arrange
            string url1 = "parkitectnexus://auth/myauthkey";

            // act
            var result1 = ParkitectNexusUrl.Parse(url1);

            Assert.AreEqual(ParkitectNexusUrlAction.Auth, result1.Action);
            Assert.IsInstanceOfType(result1.Data, typeof(ParkitectNexusAuthUrlAction));
            Assert.AreEqual("myauthkey", (result1.Data as ParkitectNexusAuthUrlAction)?.Key);
        }
    }
}
