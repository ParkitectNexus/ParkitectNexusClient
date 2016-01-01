﻿using System;

using ParkitectNexus.Data.Utilities;
using CommandLine;
namespace ParkitectNexus.Client.GTK
{
	/// <summary>
	///     Represents a collection of command line options which can be set with the execution of the application.
	/// </summary>
	public class CommandLineOptions
	{
		/// <summary>
		///     Gets the download URL of an asset file.
		/// </summary>
		[Option('d', "download")]
		public string DownloadUrl { get; set; }

		/// <summary>
		///     Gets the set-installation-path option value. Should be path to the installation path of the game if set.
		/// </summary>
		[Option("set-installation-path")]
		public string SetInstallationPath { get; set; }

		/// <summary>
		///     Gets a value indicating whether not to open the ParkitectNexus website when no download action is specified.
		/// </summary>
		[Option('s', "silent")]
		public bool Silent { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether to lauch the game.
		/// </summary>
		[Option('l', "launch")]
		public bool Launch { get; set; }

		/// <summary>
		///     Gets or sets the log level.
		/// </summary>
		/// <value>
		///     The log level.
		/// </value>
		[Option('o', "loglevel")]
		public LogLevel LogLevel { get; set; } = LogLevel.Info;
	}
}

