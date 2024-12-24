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
        /// <exception cref="InvalidOperationException">Thrown when the JSON data cannot be deserialized.</exception>
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
                // Deserialize the raw JSON data into a list of Configuration objects
                var deserializedData = JsonConvert.DeserializeObject<List<Configuration>>(rawData);

                // Check if deserialization was successful
                if ( deserializedData != null )
                {
                    // Add each configuration item to the dictionary
                    foreach ( var configurationItem in deserializedData )
                    {
                        Data[configurationItem.Name] = configurationItem;
                    }
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
