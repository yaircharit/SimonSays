using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConfigurationLoader
{
    internal class XMLConfigLoader : ConfigLoader
    {
        internal XMLConfigLoader(string configPath) : base(configPath)
        {
        }

        protected override void ParseRawData(string rawData)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rawData);

                foreach ( XmlNode section in xmlDoc.DocumentElement.ChildNodes )
                {
                    var sectionData = new Dictionary<string, object>();
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
