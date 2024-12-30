using System.Collections.Generic;
using System.IO;
using System.Xml;
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
        /// Deserilizes the raw XML data.
        /// </summary>
        protected override List<T> Deserialize(string rawData)
        {

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rawData);

            //// Check if the XML document has a root element
            //if ( xmlDoc.DocumentElement == null )
            //{
            //    throw new InvalidOperationException("The XML document is missing a root element.");
            //}

            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(xmlDoc.DocumentElement.Name));
            using ( var reader = new StringReader(xmlDoc.InnerXml) )
            {
                return (List<T>)serializer.Deserialize(reader);
            }

        }
    }
}
