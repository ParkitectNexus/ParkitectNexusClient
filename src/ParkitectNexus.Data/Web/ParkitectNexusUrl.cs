// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Net;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Represents an URL with the parkitectnexus protocol.
    /// </summary>
    public class ParkitectNexusUrl : IParkitectNexusUrl
    {
        private const string Protocol = "parkitectnexus:";
        private const string ProtocolInstructionSeparator = "/";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectNexusUrl" /> class.
        /// </summary>
        public ParkitectNexusUrl(ParkitectNexusUrlAction action, IParkitectNexusUrlAction data)
        {
            Action = action;
            Data = data;
        }

        public ParkitectNexusUrlAction Action { get; set; }
        public IParkitectNexusUrlAction Data { get; set; }

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
                throw new FormatException("invalid url format: " + input);
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
            if (!input.StartsWith(Protocol) || input.Length < Protocol.Length + 2)
                return false;

            // Trim off the protocol and any number of slashes.
            input = input.Substring(Protocol.Length).Trim('/');

            // Split parameters and url decode them.
            var parameters =
                input.Split(new[] {ProtocolInstructionSeparator}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(WebUtility.UrlDecode)
                    .ToArray();

            // Find the type asociated with the parameters.
            var action =
                typeof (ParkitectNexusUrlAction).GetEnumValues()
                    .OfType<ParkitectNexusUrlAction>()
                    .FirstOrDefault(v => v.ToString().ToLower() == parameters[0]);

            // Get data attribute from action enum value
            var attribute = action.GetCustomAttribute<ParkitectNexusUrlActionAttribute>();

            if (attribute == null)
                return false;

            // Find the proper constructor for the data container
            var constructors = attribute.Type.GetConstructors();

            var constructor =
                constructors
                    .OrderByDescending(c => c.GetParameters().Length)
                    .FirstOrDefault(c => c.GetParameters().All(p => p.ParameterType == typeof (string)) &&
                                         c.GetParameters().Length <= parameters.Length - 1);

            var data = constructor?.Invoke(parameters.Skip(1).ToArray()) as IParkitectNexusUrlAction;

            if (data == null)
                return false;

            output = new ParkitectNexusUrl(action, data);
            return true;
        }
    }
}
