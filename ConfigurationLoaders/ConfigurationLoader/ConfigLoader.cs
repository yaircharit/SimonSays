using System.Globalization;

namespace ConfigurationLoader
{
    /// <summary>
    /// Abstract class representing a configuration loader.
    /// </summary>
    public abstract class ConfigLoader
    {
        /// <summary>
        /// Path to the configuration file.
        /// </summary>
        private string configPath;

        /// <summary>
        /// Dictionary to hold configuration data.
        /// </summary>
        protected Dictionary<string, Dictionary<string, object>> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        public ConfigLoader(string configPath)
        {
            data = new Dictionary<string, Dictionary<string, object>>();
            this.configPath = configPath;

            // Read the raw data from the configuration file.
            string rawData = File.ReadAllText(configPath);
            // Parse the raw data.
            ParseRawData(rawData);
        }

        /// <summary>
        /// Parses the raw data from the configuration file and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw data as a string.</param>
        protected abstract void ParseRawData(string rawData);

        /// <summary>
        /// Gets the configuration dictionary for a given key.
        /// </summary>
        /// <param name="key">The key of the configuration.</param>
        /// <returns>A dictionary containing the configuration, or null if the key does not exist.</returns>
        public Dictionary<string, object>? GetConfiguration(string key)
        {
            if ( data.ContainsKey(key) )
            {
                return data[key];
            }
            return null;
        }

        /// <summary>
        /// Gets a value of type <typeparamref name="T"/> from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="configurationName">The name of the configuration.</param>
        /// <param name="key">The key of the value within the configuration.</param>
        /// <returns>The value of type <typeparamref name="T"/>, or the default value of <typeparamref name="T"/> if the key does not exist.</returns>
        public T? GetValue<T>(string configurationName, string key)
        {
            if ( data.ContainsKey(configurationName) )
            {
                var configuration = data[configurationName];
                if ( configuration.ContainsKey(key) )
                {
                    return (T)Convert.ChangeType(configuration[key], typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return default(T);
        }
    }
}
