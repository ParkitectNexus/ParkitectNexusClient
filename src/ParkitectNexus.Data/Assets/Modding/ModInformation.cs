// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Assets.Modding
{
    public class ModInformation
    {
        /// <summary>
        ///     Gets or sets the base directory.
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        ///     Gets or sets the compiler version.
        /// </summary>
        public string CompilerVersion { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is development.
        /// </summary>
        public bool IsDevelopment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the project.
        /// </summary>
        public string Project { get; set; }
    }
}