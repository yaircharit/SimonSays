using Newtonsoft.Json;

namespace ConfigurationLoader
{
    /*Implenetation of a configuration loader- an absract and generic class that is respnosible 
     * to load, read and deserialize a data file (Json, xml, yml, etc..)
     * 
     * To use it you need to inherit this class and Implement the 'Deserialize' function in relation with the data type.
     * After creating a new class it should be added to the case list in the LoadConfiguration<T> function below.
     * 
     * The user can pass the wanted object type to deserialize to (type T).
     * Allowing the usage of this ConfigLoader<T> class in many versatile cases.
     * 
     * The loader and deserialization are returning a list of objects (and not a dictionary) to better handle the deserialization (what would be the key in a list?)
     * The returned list can be mapped by the user afterwards as needed.
     * 
     * There could be different approach- to pass on the Deserialize function from outside of this library.
     * But in my opinion this class library should handle this internaly since there shouldn't be a difference between parsing several
     * configs of the same type (json for example) even if they are for different applications.
     */


    /// <summary>
    /// Abstract and generic class representing a configuration loader.
    /// </summary>
    public abstract class ConfigLoader<T> where T : class
    {
        protected string configPath;

        public static ConfigLoader<T> Instance { get; private set; }

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
