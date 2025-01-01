using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigurationLoader
{
    /// <summary>
    /// Abstract and generic class representing a configuration loader.
    /// </summary>
    public abstract class ConfigLoader<T> where T : class
    {
        protected string configPath;

        private static ConfigLoader<T> instance = null;

        private static readonly object padlock = new object();

        public static ConfigLoader<T> Instance
        {
            get {
                lock ( padlock )
                {
                    if ( instance == null )
                    {
                        throw new InvalidOperationException("Instance not created. Call LoadConfig method first.");
                    }
                    return instance;
                }
            }
            private set {
                lock ( padlock )
                {
                    instance = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigLoader"/> class.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        protected ConfigLoader(string configPath)
        {
            // Read the raw data from the configuration file.
            this.configPath = configPath;
        }

        /// <summary>
        /// Deserializes the raw data
        /// </summary>
        /// <param name="rawData">Data to deserialize</param>
        /// <returns>A list of all objects </returns>
        protected abstract List<T> Deserialize(string rawData);

        /// <summary>
        /// Parses the raw data from the configuration file
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        /// <exception cref="NotSupportedException">Thrown when the path is in an invalid format.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <returns>A list of objects from the configuraion file</returns>
        private List<T> ParseRawData()
        {

            string rawData = File.ReadAllText(configPath); // Not in try/catch to throw the appropriate exception of File.ReadAllText

            try
            {
                return Deserialize(rawData);
            }
            catch ( Exception e )
            {
                // Throw an exception if deserialization failed
                throw new InvalidOperationException("Failed to deserialize configuration data.", e);
            };
        }

        /// <summary>
        /// Loads the appropriate configuration loader based on the file extension.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        /// <returns>An instance of a <see cref="ConfigLoader"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown when the configuration file format is unsupported.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        public static List<T> LoadConfig(string configPath)
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
                    throw new NotSupportedException("Unsupported configuration file format.");
            }
            return Instance.ParseRawData();
        }
    }
}
