using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        throw new InvalidOperationException("Instance not created. Call LoadConfig method first.");
                    }
                    return instance;
                }
            }
            private set
            {
                lock (padlock)
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
        /// Gets the raw data for this configuration loader. Override in subclasses to change the source
        /// (for example: remote fetch from Firebase Remote Config).
        /// </summary>
        /// <returns>Raw configuration as string.</returns>
        protected virtual async Task<string> GetRawDataAsync()
        {
            return await Task.Run(() => File.ReadAllText(configPath));
        }

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
        private async Task<List<T>> ParseRawDataAsync()
        {

            // Allow subclasses to override how raw data is obtained (file, remote, etc.)
            string rawData = await GetRawDataAsync(); // Not in try/catch to allow underlying IO exceptions to bubble up

            try
            {
                return Deserialize(rawData);
            }
            catch (Exception e)
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
        public static async Task<List<T>> LoadConfigAsync(string configPath)
        {
            switch (Path.GetExtension(configPath).ToLowerInvariant())
            {
                case ".json":
                    Instance = new JsonConfigLoader<T>(configPath);
                    break;
                case ".xml":
                    Instance = new XMLConfigLoader<T>(configPath);
                    break;
                case ".firebase":
                    Instance = new FirebaseConfigLoader<T>(configPath.Substring(0, configPath.Length - ".firebase".Length));
                    break;
                default:
                    throw new NotSupportedException("Unsupported configuration file format.");
            }

            return await Instance.ParseRawDataAsync();
        }
    }
}
