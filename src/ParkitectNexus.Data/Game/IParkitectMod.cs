// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Collections.Generic;
using System.IO;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides the functionality of a Parkitect mod provided by ParkitectNexus.
    /// </summary>
    public interface IParkitectMod
    {
        /// <summary>
        ///     Gets the parkitect instance this mod was installed to.
        /// </summary>
        IParkitect Parkitect { get; }
        
        /// <summary>
        ///     Gets or sets the base directory.
        /// </summary>
        string BaseDir { get; set; }

        /// <summary>
        ///     Gets or sets the compiler version.
        /// </summary>
        string CompilerVersion { get; set; }

        /// <summary>
        ///     Gets or sets the code files.
        /// </summary>
        IList<string> CodeFiles { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is development.
        /// </summary>
        bool IsDevelopment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is installed.
        /// </summary>
        bool IsInstalled { get; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        string InstallationPath { get; set; }

        /// <summary>
        ///     Gets or sets the project.
        /// </summary>
        string Project { get; set; }

        /// <summary>
        ///     Gets or sets the tag.
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        ///     Gets or sets the referenced assemblies.
        /// </summary>
        IList<string> ReferencedAssemblies { get; set; }

        /// <summary>
        ///     Gets or sets the repository.
        /// </summary>
        string Repository { get; set; }

        /// <summary>
        ///     Opens the log for this mod instance.
        /// </summary>
        /// <returns>A stream writer for mod related logging.</returns>
        StreamWriter OpenLog();

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        void Save();

        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        void Delete();

        /// <summary>
        ///     Compiles this instance.
        /// </summary>
        /// <returns>true on success; false otherwise.</returns>
        bool Compile();
    }
}