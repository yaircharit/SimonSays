using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        /// <param name="configPath">The path to the XML configuration file.</param>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file could not be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while opening the configuration file.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the caller does not have the required permission to access the configuration file.</exception>
        /// <exception cref="ArgumentException">Thrown when the path is invalid.</exception>
        /// <exception cref="PathTooLongException">Thrown when the specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException">Thrown when the path is in an invalid format.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the JSON data cannot be deserialized.</exception>
        internal XMLConfigLoader(string configPath) : base(configPath)
        {
        }

        /// <summary>
        /// Parses the raw XML data and populates the configuration dictionary.
        /// </summary>
        /// <param name="rawData">The raw XML data as a string.</param>
        /// <exception cref="InvalidOperationException">Thrown when the XML data cannot be parsed.</exception>
        protected override void ParseRawData(string rawData)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rawData);

                // Iterate through each section in the XML document
                foreach ( XmlNode section in xmlDoc.DocumentElement.ChildNodes )
                {
                    var sectionData = new Dictionary<string, object>();
                    // Iterate through each node in the section
                    foreach ( XmlNode node in section.ChildNodes )
                    {
                        sectionData[node.Name] = node.InnerText;
                    }
                    data[section.Name] = sectionData;
                }
            }
            catch ( XmlException e )
            {
                throw new InvalidOperationException("Failed to parse XML data.", e);
            }
        }
    }
}
