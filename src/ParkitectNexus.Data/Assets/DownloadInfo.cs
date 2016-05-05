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

namespace ParkitectNexus.Data.Assets
{
    public struct DownloadInfo
    {
        public DownloadInfo(string url, string repository, string tag)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            Url = url;
            Repository = repository;
            Tag = tag;
        }

        public string Url { get; }

        public string Repository { get; }

        public string Tag { get; }
    }
}