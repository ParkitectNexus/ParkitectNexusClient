﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.Client
{
    public interface IParkitectNexusWebFactory
    {
        IParkitectNexusWebClient CreateWebClient();
    }
}