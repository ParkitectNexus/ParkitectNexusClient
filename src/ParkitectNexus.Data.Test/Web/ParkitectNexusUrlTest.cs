// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
