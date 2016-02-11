// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Contains the properties of a resource.
    /// </summary>
    public interface IApiResource
    {
        /// <summary>
        ///     Gets or sets the identifier of this resource.
        /// </summary>
        int Id { get; set; }
    }
}