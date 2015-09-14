// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Net;

namespace ParkitectNexusClient
{
    /// <summary>
    ///     Represents an URL with the parkitectnexus protocol.
    /// </summary>
    internal class ParkitectNexusUrl
    {
        private const string Protocol = "parkitectnexus:";
        private const string ProtocolInstructionSeparator = "|";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectNexusUrl" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="fileHash">The file hash.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">invalid file hash</exception>
        public ParkitectNexusUrl(string name, ParkitectAssetType assetType, string fileHash)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (fileHash == null) throw new ArgumentNullException(nameof(fileHash));
            if (!ParkitectNexus.IsValidFileHash(fileHash))
                throw new ArgumentException("invalid file hash", nameof(fileHash));

            Name = name;
            AssetType = assetType;
            FileHash = fileHash;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the type of the asset.
        /// </summary>
        public ParkitectAssetType AssetType { get; }

        /// <summary>
        ///     Gets the file hash.
        /// </summary>
        public string FileHash { get; }

        #region Overrides of Object

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return
                $"{Protocol}//{Name}{ProtocolInstructionSeparator}{AssetType}{ProtocolInstructionSeparator}{FileHash}";
        }

        #endregion

        /// <summary>
        ///     Parses the specified input to an instance of <see cref="ParkitectNexusUrl" />
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The parsed instance.</returns>
        /// <exception cref="FormatException">Thrown if invalid url format.</exception>
        public static ParkitectNexusUrl Parse(string input)
        {
            ParkitectNexusUrl output;
            if (!TryParse(input, out output))
                throw new FormatException("invalid url format");
            return output;
        }

        /// <summary>
        ///     Tries the parse the specified input to an instance of <see cref="ParkitectNexusUrl" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>true if successful; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
        public static bool TryParse(string input, out ParkitectNexusUrl output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            output = null;

            // Make sure the input url starts with the parkitect: protocol.
            if (!input.StartsWith(Protocol) || input.Length < Protocol.Length + 4)
                return false;

            // Trim off the protocol and any number of slashes.
            input = input.Substring(Protocol.Length).Trim('/');

            var parts = input.Split(new[] {ProtocolInstructionSeparator}, StringSplitOptions.None);

            // Make sure the url consists of 3 parts.
            if (parts.Length != 3)
                return false;

            // Parse the asset type.
            var assetTypeString =
                Enum.GetNames(typeof (ParkitectAssetType)).FirstOrDefault(n => n.ToLower() == parts[1].ToLower());
            ParkitectAssetType assetType;
            if (!Enum.TryParse(assetTypeString, out assetType))
                return false;

            // Decode HTML entities in the name of the asset.
            var name = WebUtility.UrlDecode(parts[0]);

            // Make sure the file hash is valid.
            var fileHash = parts[2];
            if (!ParkitectNexus.IsValidFileHash(fileHash))
                return false;

            // Return an instance of the url.
            output = new ParkitectNexusUrl(name, assetType, fileHash);
            return true;
        }
    }
}