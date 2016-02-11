// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Tasks.Prefab;

namespace ParkitectNexus.Data.Web.Models
{
    [UrlActionTask(typeof (AuthenticateTask))]
    public class AuthUrlAction : IUrlAction
    {
        public AuthUrlAction(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        public string Key { get; }
    }
}