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

using Xwt.Drawing;

namespace ParkitectNexus.Client.Base
{
    public class UIImageProvider<TImageType> where TImageType : class
    {
        public TImageType this[string key]
        {
            get
            {
                if (typeof (TImageType) == typeof (Image))
                    return Image.FromResource(GetType(), $"ParkitectNexus.Client.Base.Resources.{key}") as TImageType;

                if (typeof (TImageType) == typeof (System.Drawing.Image))
                {
                    using (var str = GetType().Assembly.GetManifestResourceStream($"ParkitectNexus.Client.Base.Resources.{key}"))
                        return System.Drawing.Image.FromStream(str) as TImageType;
                }

                return null;
            }
        }
    }
}
