using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigLoaderUnitTests
{
    [TestClass]
    public sealed class XMLConfigTester : ConfigTesterBase
    {
        protected override string ConfigFilePath => "../../../config.xml";
        protected override string NonCompConfigFilePath => "../../../non_compatible_config.xml";
        protected override string NonExistingConfigFilePath => "non_existing_config.xml";
    }
}
