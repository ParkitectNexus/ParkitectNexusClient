// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Net;

namespace ParkitectNexusInstaller
{
    internal class ParkitectNexusUrl
    {
        private const string Protocol = "parkitectnexus:";
        private const string ProtocolInstructionSeparator = "|";

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

        public string Name { get; }
        public ParkitectAssetType AssetType { get; }
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

        public static ParkitectNexusUrl Parse(string input)
        {
            ParkitectNexusUrl output;
            if (!TryParse(input, out output))
                throw new FormatException("invalid url format");
            return output;
        }

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

            var name = WebUtility.HtmlDecode(parts[0]);
            var fileHash = parts[2];

            // Make sure the file hash is valid.
            if (!ParkitectNexus.IsValidFileHash(fileHash))
                return false;

            // Return an instance of the url.
            output = new ParkitectNexusUrl(name, assetType, fileHash);
            return true;
        }
    }
}