using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigLoaderUnitTests
{
    [TestClass]
    public sealed class JSONConfigTester : ConfigTesterBase
    {
        protected override string ConfigFilePath => "../../../config.json";
        protected override string NonCompConfigFilePath => "../../../non_compatible_config.json";
        protected override string NonExistingConfigFilePath => "non_existing_config.json";
    }
}
