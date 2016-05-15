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

using System;
using System.Drawing;
using System.Threading.Tasks;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    public class ApiImage : ApiResource
    {
        public async Task<Bitmap> Get()
        {
            if (Url == null)
                return null;

            try
            {
                using (var webclient = ObjectFactory.GetInstance<INexusWebClientFactory>().CreateWebClient())
                {
                    using (var stream = await webclient.OpenReadTaskAsync(Url))
                    using (var bmp = new Bitmap(stream))
                        return bmp.Clone() as Bitmap;
                }
            }
            catch (Exception e)
            {
                var log = ObjectFactory.GetInstance<ILogger>();

                log.WriteLine($"Failed to get image from {Url}");
                log.WriteException(e);

                return null;
            }
        }
    }
}