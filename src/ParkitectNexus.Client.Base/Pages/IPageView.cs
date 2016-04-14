// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Client.Base.Pages
{
    public interface IPageView
    {
        string DisplayName { get; }

        event EventHandler DisplayNameChanged;
    }
}
