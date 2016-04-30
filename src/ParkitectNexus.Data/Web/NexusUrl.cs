// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
    public class NexusUrl : INexusUrl
    {
        private const string Protocol = "parkitectnexus:";
        private const string ProtocolInstructionSeparator = "/";

        /// <summary>
        ///     Initializes a new instance of the <see cref="NexusUrl" /> class.
        /// </summary>
        public NexusUrl(UrlAction action, IUrlAction data)
        {
            Action = action;
            Data = data;
        }

        public UrlAction Action { get; set; }

        public IUrlAction Data { get; set; }

        /// <summary>
        ///     Parses the specified input to an instance of <see cref="NexusUrl" />
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The parsed instance.</returns>
        /// <exception cref="FormatException">Thrown if invalid url format.</exception>
        public static NexusUrl Parse(string input)
        {
            NexusUrl output;
            if (!TryParse(input, out output))
                throw new FormatException("invalid url format: " + input);
            return output;
        }

        /// <summary>
        ///     Tries the parse the specified input to an instance of <see cref="NexusUrl" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>true if successful; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
        public static bool TryParse(string input, out NexusUrl output)
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
                typeof (UrlAction).GetEnumValues()
                    .OfType<UrlAction>()
                    .FirstOrDefault(v => v.ToString().ToLower() == parameters[0]);

            // Get data attribute from action enum value
            var attribute = action.GetCustomAttribute<UrlActionAttribute>();

            if (attribute == null)
                return false;

            // Find the proper constructor for the data container
            var constructors = attribute.Type.GetConstructors();

            var constructor =
                constructors
                    .OrderByDescending(c => c.GetParameters().Length)
                    .FirstOrDefault(c => c.GetParameters().All(p => p.ParameterType == typeof (string)) &&
                                         c.GetParameters().Length <= parameters.Length - 1);

            var data = constructor?.Invoke(parameters.Skip(1).ToArray()) as IUrlAction;

            if (data == null)
                return false;

            output = new NexusUrl(action, data);
            return true;
        }
    }
}