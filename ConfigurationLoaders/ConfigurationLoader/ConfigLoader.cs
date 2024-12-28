namespace ConfigurationLoader
{
    /// <summary>
    /// Abstract class representing a configuration loader.
    /// </summary>
    public abstract class ConfigLoader<T> where T : class
    {
        /// <summary>
        /// Dictionary to hold configuration data.
        /// </summary>
        public Dictionary<string, T> Data { get; protected set; }

        public static ConfigLoader<T> Instance { get; private set; }

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
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        protected ConfigLoader(string configPath)
        {
            Data = new Dictionary<string, T>();

            // Read the raw data from the configuration file.
            string rawData = File.ReadAllText(configPath);
            // Parse the raw data.
            ParseRawData(rawData);
        }

        /// <summary>
        /// Parses the raw data from the configuration file and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw data as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        protected abstract void ParseRawData(string rawData);

        /// <summary>
        /// Loads the appropriate configuration loader based on the file extension.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <returns>An instance of a <see cref="ConfigLoader"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the configuration file format is unsupported.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        public static Dictionary<string, T> LoadConfig(string configPath)
        {
            switch ( Path.GetExtension(configPath).ToLowerInvariant() )
            {
                case ".json":
                    Instance = new JsonConfigLoader<T>(configPath);
                    break;
                case ".xml":
                    Instance = new XMLConfigLoader<T>(configPath);
                    break;
                default:
                    throw new ArgumentException("Unsupported configuration file format.");
            }
            return Instance.Data;
        }
    }
}
