// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Web
{
    public class ParkitectNexusUrlActionAttribute : Attribute
    {
        public ParkitectNexusUrlActionAttribute(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!typeof (IUrlAction).IsAssignableFrom(type))
                throw new ArgumentException("type must implement IUrlAction", nameof(type));
            Type = type;
        }

        public Type Type { get; }
    }
}
