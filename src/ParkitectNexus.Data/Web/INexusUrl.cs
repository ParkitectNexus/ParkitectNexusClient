// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Web
{
    public interface INexusUrl
    {
        UrlAction Action { get; }

        IUrlAction Data { get; }
    }
}