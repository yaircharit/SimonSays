using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLoader
{
    /// <summary>
    /// Class responsible for loading and parsing JSON configuration files.
    /// </summary>
    internal class JsonConfigLoader : ConfigLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the JSON configuration file.</param>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException">Thrown when the path is in an invalid format.</exception>
        /// <exception cref="JsonException">Thrown when the JSON data cannot be deserialized.</exception>
        internal JsonConfigLoader(string configPath) : base(configPath)
        {
        }

        /// <summary>
        /// Parses the raw JSON data and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw JSON data as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the JSON data cannot be deserialized.</exception>
        protected override void ParseRawData(string rawData)
        {
            try
            {
                // Deserialize the raw JSON data into a dictionary
                var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(rawData);

                // Check if deserialization was successful
                if ( deserializedData != null )
                {
                    // Assign the deserialized data to the base class's data field
                    data = deserializedData;
                }
            }
            catch ( JsonException e )
            {
                // Throw an exception if deserialization failed
                throw new InvalidOperationException("Failed to deserialize JSON data.", e);
            };
        }
    }
}
