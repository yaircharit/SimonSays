using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigurationLoader
{
    /// <summary>
    /// Class responsible for loading and parsing XML configuration files.
    /// </summary>
    internal class XMLConfigLoader : ConfigLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XMLConfigLoader"/> class.
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
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rawData);

                // Check if the XML document has a root element
                if ( xmlDoc.DocumentElement == null )
                {
                    throw new InvalidOperationException("The XML document is missing a root element.");
                }

                // Iterate through each child node of the root element
                foreach ( XmlNode section in xmlDoc.DocumentElement.ChildNodes )
                {
                    var serializer = new XmlSerializer(typeof(Configuration));
                    using ( var reader = new StringReader(section.OuterXml) )
                    {
                        // Deserialize the XML node into a Configuration object
                        var configuration = (Configuration?)serializer.Deserialize(reader);
                        if ( configuration == null )
                        {
                            throw new InvalidOperationException($"Failed to deserialize configuration for section {section.Name}.");
                        }

                        // Add the configuration to the dictionary
                        Data[configuration.Name] = configuration;
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
