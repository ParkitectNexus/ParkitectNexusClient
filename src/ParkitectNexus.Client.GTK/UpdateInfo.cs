using System;
using Newtonsoft.Json;

namespace ParkitectNexus.Client.GTK
{
	[JsonObject]
	public class UpdateInfo
	{
		/// <summary>
		///     Gets or sets the version.
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		///     Gets or sets the download URL.
		/// </summary>
		[JsonProperty("download_url")]
		public string DownloadUrl { get; set; }
	}
}

