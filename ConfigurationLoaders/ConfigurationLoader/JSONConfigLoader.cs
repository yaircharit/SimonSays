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
    public class JSONConfigLoader : ConfigLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the JSON configuration file.</param>
        public JSONConfigLoader(string configPath) : base(configPath)
        {
        }

        /// <summary>
        /// Parses the raw JSON data and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw JSON data as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the JSON data cannot be deserialized.</exception>
        protected override void ParseRawData(string rawData)
        {
            // Deserialize the raw JSON data into a dictionary
            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(rawData);

            // Check if deserialization was successful
            if ( deserializedData != null )
            {
                // Assign the deserialized data to the base class's data field
                data = deserializedData;
            } else
            {
                // Throw an exception if deserialization failed
                throw new InvalidOperationException("Failed to deserialize data");
            }
        }
    }
}
