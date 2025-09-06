using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Core.Configs
{
    /// <summary>
    /// Class responsible for loading and deserialize JSON configuration files.
    /// </summary>
    public class JsonConfigLoader<T> : ConfigLoader<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the JSON configuration file.</param>
        public JsonConfigLoader(string configPath) : base(configPath)
        {
        }

        /// <summary>
        /// Deserializes the raw JSON data
        /// </summary>
        /// <param name="rawData">The raw JSON data as a string.</param>
        /// <returns>A list of objects of type <see cref="T"/></returns>
        /// <exception cref="JsonException">Thrown when the JSON data cannot be deserialized to a list or dictionary</exception>
        protected override List<T> Deserialize(string rawData)
        {
            try
            {
                // (this is the 'default' because there shouldn't be duplicates in a configuration file- hence a dictionary and not a list)
                return JsonConvert.DeserializeObject<Dictionary<string, T>>(rawData).Select(item => item.Value).ToList();

            }
            catch ( JsonException )
            {
                // Fallback: If can't desrialize to dictionary try a list
                return JsonConvert.DeserializeObject<List<T>>(rawData);
            }
        }
    }
}
