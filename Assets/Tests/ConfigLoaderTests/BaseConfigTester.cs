using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Core.Configs;

namespace Tests.ConfigLoaderTests
{
    public abstract class ConfigTesterBase : ScriptableObject
    {
        protected abstract string FileExtension { get; }

        private string ConfigsFolderPath => Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Tests\ConfigLoaderTests\ConfigFiles");

        private string _configFileName = "config";
        public string ConfigFileName => $"{_configFileName}.{FileExtension}";


        private string _nonCompConfigFileName = "non_compatible_config";
        public string NonCompConfigFileName => $"{_nonCompConfigFileName}.{FileExtension}";


        private string _nonExistingConfigFileeName = "non_existing_config";
        public string NonExistingConfigFileeName => $"{_nonExistingConfigFileeName}.{FileExtension}";

        private string GetFullPathToFile(string filename)
        {
            return Path.Combine(ConfigsFolderPath, filename);
        }

        protected Dictionary<string, TestsAppConfig> configData = null!;

        [SetUp]
        public void Setup()
        {
            var temp = GetFullPathToFile(ConfigFileName);
            // Await the Task to get the List<AppConfig> before calling ToDictionary
            var configList = ConfigManager<TestsAppConfig>.LoadConfigsAsync(GetFullPathToFile(ConfigFileName)).GetAwaiter().GetResult();
            configData = configList.ToDictionary(config => config.Name, config => config);
        }

        [Test]
        public void TestLoad()
        {
            var configKeys = configData.Keys;
            CollectionAssert.Contains(configKeys, "Easy");
            CollectionAssert.Contains(configKeys, "Medium");
            CollectionAssert.Contains(configKeys, "Hard");
            CollectionAssert.DoesNotContain(configKeys, "Extreme");
        }

        [Test]
        public void TestGetValue_Easy()
        {
            var config = configData["Easy"];
            Assert.IsNotNull(config);
            Assert.AreEqual(4, config.GameButtons);
            Assert.AreEqual(1, config.PointsEachStep);
            Assert.AreEqual(50, config.GameTime);
            Assert.AreEqual(true, config.RepeatMode);
            Assert.AreEqual(1.0f, config.GameSpeed);
            Assert.AreEqual(1, config.Index);
        }

        [Test]
        public void TestGetValue_Medium()
        {
            var config = configData["Medium"];
            Assert.IsNotNull(config);
            Assert.AreEqual(5, config.GameButtons);
            Assert.AreEqual(2, config.PointsEachStep);
            Assert.AreEqual(45, config.GameTime);
            Assert.AreEqual(true, config.RepeatMode);
            Assert.AreEqual(1.25f, config.GameSpeed);
            Assert.AreEqual(2, config.Index);
        }

        [Test]
        public void TestGetValue_Hard()
        {
            var config = configData["Hard"];
            Assert.IsNotNull(config);
            Assert.AreEqual(6, config.GameButtons);
            Assert.AreEqual(3, config.PointsEachStep);
            Assert.AreEqual(30, config.GameTime);
            Assert.AreEqual(false, config.RepeatMode);
            Assert.AreEqual(1.5f, config.GameSpeed);
            Assert.AreEqual(3, config.Index);
        }

        [Test]
        public void TestLoadNonExistingFile()
        {
            Assert.That(() =>
                ConfigManager<TestsAppConfig>.LoadConfigsAsync(GetFullPathToFile(NonExistingConfigFileeName)),
                Throws.TypeOf<FileNotFoundException>()
            );
        }

        [Test]
        public void TestLoadNonCompatibleFile()
        {
            Assert.That(() =>
                ConfigManager<TestsAppConfig>.LoadConfigsAsync(GetFullPathToFile(NonCompConfigFileName)),
                Throws.TypeOf<InvalidOperationException>()
            );
        }
    }
}
