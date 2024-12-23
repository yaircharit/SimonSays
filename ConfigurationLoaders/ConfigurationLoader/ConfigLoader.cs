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
        private readonly string configPath;

        /// <summary>
        /// Dictionary to hold configuration data.
        /// </summary>
        protected Dictionary<string, Dictionary<string, object>> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException">Thrown when the path is in an invalid format.</exception>
        protected ConfigLoader(string configPath)
        {
            data = new Dictionary<string, Dictionary<string, object>>();
            this.configPath = configPath;

            ReadConfigurationFile();
        }

        /// <summary>
        /// Reads the configuration file and parses its content.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException">Thrown when the path is in an invalid format.</exception>
        private void ReadConfigurationFile()
        {
            // Read the raw data from the configuration file.
            string rawData = File.ReadAllText(this.configPath);
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
        private Dictionary<string, object>? GetConfiguration(string key)
        {
            if ( data.ContainsKey(key) )
            {
                return data[key];
            }
            return null;
        }

        /// <summary>
        /// Gets a list of all configuration keys.
        /// </summary>
        /// <returns>A list of all configuration keys.</returns>
        public List<string> GetConfigurationKeys()
        {
            return data.Keys.ToList();
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
            var configuration = GetConfiguration(configurationName);
            if ( configuration != null && configuration.ContainsKey(key) )
            {
                return (T)Convert.ChangeType(configuration[key], typeof(T), CultureInfo.InvariantCulture);
            }

            return default(T);
        }

        /// <summary>
        /// Loads the appropriate configuration loader based on the file extension.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <returns>An instance of a <see cref="ConfigLoader"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the configuration file format is unsupported.</exception>
        public static ConfigLoader LoadConfig(string configPath)
        {
            switch ( Path.GetExtension(configPath).ToLowerInvariant() )
            {
                case ".json":
                    return new JsonConfigLoader(configPath);
                case ".xml":
                    return new XMLConfigLoader(configPath);
                default:
                    throw new ArgumentException("Unsupported configuration file format.");
            }
        }
    }
}
