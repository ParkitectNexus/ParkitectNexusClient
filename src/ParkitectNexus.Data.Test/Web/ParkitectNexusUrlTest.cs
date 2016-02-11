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
            var result1 = NexusUrl.Parse(url1);

            Assert.AreEqual(UrlAction.Install, result1.Action);
            Assert.IsInstanceOfType(result1.Data, typeof (InstallUrlAction));
            Assert.AreEqual("testidentifier", (result1.Data as InstallUrlAction)?.Id);
        }

        [TestMethod]
        public void TryParseAuthTest()
        {
            // arrange
            string url1 = "parkitectnexus://auth/myauthkey";

            // act
            var result1 = NexusUrl.Parse(url1);

            Assert.AreEqual(UrlAction.Auth, result1.Action);
            Assert.IsInstanceOfType(result1.Data, typeof (AuthUrlAction));
            Assert.AreEqual("myauthkey", (result1.Data as AuthUrlAction)?.Key);
        }
    }
}