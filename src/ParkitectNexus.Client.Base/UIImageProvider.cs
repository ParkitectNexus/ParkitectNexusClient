// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Xwt.Drawing;

namespace ParkitectNexus.Client.Base
{
    public class UIImageProvider
    {
        public Image this[string key] => Image.FromResource(GetType(), $"ParkitectNexus.Client.Base.Resources.{key}");
    }
}
