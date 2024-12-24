using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConfigurationLoader
{
    /// <summary>
    /// Class responsible for loading and parsing XML configuration files.
    /// </summary>
    internal class XMLConfigLoader<T> : ConfigLoader<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XMLConfigLoader{T}"/> class.
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        internal XMLConfigLoader(string configPath) : base(configPath)
        {
        }

        /// <summary>
        /// Parses the raw XML data and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw XML data as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the raw data cannot be deserialized.</exception>
        protected override void ParseRawData(string rawData)
        {
            try
            {
                var xDoc = XDocument.Parse(rawData);

                // Check if the XML document has a root element
                if ( xDoc.Root == null )
                {
                    throw new InvalidOperationException("The XML document is missing a root element.");
                }

                // Iterate through each child node of the root element
                foreach ( var section in xDoc.Root.Elements() )
                {
                    var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(section.Name.LocalName));
                    using ( var reader = new StringReader(section.ToString()) )
                    {
                        // Deserialize the XML element into a T object
                        var configuration = (T)serializer.Deserialize(reader);
                        if ( configuration == null )
                        {
                            throw new InvalidOperationException($"Failed to deserialize configuration for section {section.Name}.");
                        }

                        // Add the configuration to the dictionary
                        Data[section.Name.LocalName] = configuration;
                    }
                }
            }
            catch ( Exception ex )
            {
                throw new InvalidOperationException("Failed to parse XML data.", ex);
            }
        }
    }
}
