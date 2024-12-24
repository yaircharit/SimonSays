using ConfigurationLoader;

namespace ConfigLoaderUnitTests
{
    public abstract class ConfigTesterBase
    {
        protected abstract string ConfigFilePath { get; }
        protected abstract string NonCompConfigFilePath { get; }
        protected abstract string NonExistingConfigFilePath { get; }
        protected Dictionary<string, Configuration> configData = null!;

        [TestInitialize]
        public void Setup()
        {
            configData = ConfigLoader.LoadConfig(ConfigFilePath);
        }

        [TestMethod]
        public void TestLoad()
        {
            var configKeys = configData.Keys;
            CollectionAssert.Contains(configKeys, "Easy");
            CollectionAssert.Contains(configKeys, "Medium");
            CollectionAssert.Contains(configKeys, "Hard");
            CollectionAssert.DoesNotContain(configKeys, "Extreme");
        }

        [TestMethod]
        public void TestGetValue_Easy()
        {
            var config = configData["Easy"];
            Assert.IsNotNull(config);
            Assert.AreEqual(4, config.GameButtons);
            Assert.AreEqual(1, config.PointsEachStep);
            Assert.AreEqual(50, config.GameTime);
            Assert.AreEqual(true, config.RepeatMode);
            Assert.AreEqual(1.0f, config.GameSpeed);
        }

        [TestMethod]
        public void TestGetValue_Medium()
        {
            var config = configData["Medium"];
            Assert.IsNotNull(config);
            Assert.AreEqual(5, config.GameButtons);
            Assert.AreEqual(2, config.PointsEachStep);
            Assert.AreEqual(45, config.GameTime);
            Assert.AreEqual(true, config.RepeatMode);
            Assert.AreEqual(1.25f, config.GameSpeed);
        }

        [TestMethod]
        public void TestGetValue_Hard()
        {
            var config = configData["Hard"];
            Assert.IsNotNull(config);
            Assert.AreEqual(6, config.GameButtons);
            Assert.AreEqual(3, config.PointsEachStep);
            Assert.AreEqual(30, config.GameTime);
            Assert.AreEqual(false, config.RepeatMode);
            Assert.AreEqual(1.5f, config.GameSpeed);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestLoadNonExistingFile()
        {
            ConfigLoader.LoadConfig(NonExistingConfigFilePath);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadNonCompatibleFile()
        {
            ConfigLoader.LoadConfig(NonCompConfigFilePath);
        }
    }
}
