using ConfigurationLoader;

namespace ConfigLoaderUnitTests
{
    /// <summary>
    /// Unit tests for the JSON configuration loader.
    /// </summary>
    [TestClass]
    public sealed class JSONConfigTester
    {
        private const string ConfigFilePath = "../../../config.json";
        private const string NonCompConfigFilePath = "../../../non_compatible_config.json";
        private const string NonExistingConfigFilePath = "non_existing_config.json";
        private ConfigLoader configLoader = null!;

        /// <summary>
        /// Initializes the configuration loader before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            configLoader = new JSONConfigLoader(ConfigFilePath);
            
        }

        /// <summary>
        /// Tests loading configurations and verifies that the expected configurations are present.
        /// </summary>
        [TestMethod]
        public void TestLoad()
        {
            Assert.IsNotNull(configLoader.GetConfiguration("Easy"));
            Assert.IsNotNull(configLoader.GetConfiguration("Medium"));
            Assert.IsNotNull(configLoader.GetConfiguration("Hard"));
            Assert.IsNull(configLoader.GetConfiguration("Extreme"));
        }

        /// <summary>
        /// Tests retrieving values from the "Easy" configuration.
        /// </summary>
        [TestMethod]
        public void TestGetValue_Easy()
        {
            Assert.AreEqual(4, configLoader.GetValue<int>("Easy", "GameButtons"));
            Assert.AreEqual(1, configLoader.GetValue<int>("Easy", "PointsEachStep"));
            Assert.AreEqual(50, configLoader.GetValue<int>("Easy", "GameTime"));
            Assert.AreEqual(true, configLoader.GetValue<bool>("Easy", "RepeatMode"));
            Assert.AreEqual(1.0f, configLoader.GetValue<float>("Easy", "GameSpeed"));
        }

        /// <summary>
        /// Tests retrieving values from the "Medium" configuration.
        /// </summary>
        [TestMethod]
        public void TestGetValue_Medium()
        {
            Assert.AreEqual(5, configLoader.GetValue<int>("Medium", "GameButtons"));
            Assert.AreEqual(2, configLoader.GetValue<int>("Medium", "PointsEachStep"));
            Assert.AreEqual(45, configLoader.GetValue<int>("Medium", "GameTime"));
            Assert.AreEqual(true, configLoader.GetValue<bool>("Medium", "RepeatMode"));
            Assert.AreEqual(1.25f, configLoader.GetValue<float>("Medium", "GameSpeed"));
        }

        /// <summary>
        /// Tests retrieving values from the "Hard" configuration.
        /// </summary>
        [TestMethod]
        public void TestGetValue_Hard()
        {
            Assert.AreEqual(6, configLoader.GetValue<int>("Hard", "GameButtons"));
            Assert.AreEqual(3, configLoader.GetValue<int>("Hard", "PointsEachStep"));
            Assert.AreEqual(30, configLoader.GetValue<int>("Hard", "GameTime"));
            Assert.AreEqual(false, configLoader.GetValue<bool>("Hard", "RepeatMode"));
            Assert.AreEqual(1.5f, configLoader.GetValue<float>("Hard", "GameSpeed"));
        }

        /// <summary>
        /// Tests loading a non-existing configuration file and expects an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestLoadNonExistingFile()
        {
            var nonExistingConfigLoader = new JSONConfigLoader(NonExistingConfigFilePath);
        }
        /// <summary>
        /// Tests loading a non-compatible JSON file and expects a JsonException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadNonCompatibleJsonFile()
        {
            var nonCompatibleConfigLoader = new JSONConfigLoader(NonCompConfigFilePath);
        }


    }
}

