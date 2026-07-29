using System;
using System.IO;
using NUnit.Framework;
using BaseArchitecture.Core;

namespace BaseArchitecture.Tests
{
    public class TestSaveData : ISaveData
    {
        public int Score;
        public string Name;
    }

    [TestFixture]
    public class PersistenceManagerTests
    {
        private string _tempDirectory;
        private string _filePath;
        private PersistenceManager _persistenceManager;

        [SetUp]
        public void Setup()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _filePath = Path.Combine(_tempDirectory, "SaveData.json");
            _persistenceManager = new PersistenceManager(_filePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDirectory))
                Directory.Delete(_tempDirectory, true);
        }

        [Test]
        public void Load_WithMissingKey_ReturnsNewInstance()
        {
            var result = _persistenceManager.Load<TestSaveData>("missing");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Score);
            Assert.IsNull(result.Name);
        }

        [Test]
        public void Exists_WithMissingKey_ReturnsFalse()
        {
            Assert.IsFalse(_persistenceManager.Exists("missing"));
        }

        [Test]
        public void SaveThenLoad_RoundTripsData()
        {
            var data = new TestSaveData { Score = 42, Name = "Player" };

            _persistenceManager.Save("progress", data);
            var loaded = _persistenceManager.Load<TestSaveData>("progress");

            Assert.IsTrue(_persistenceManager.Exists("progress"));
            Assert.AreEqual(42, loaded.Score);
            Assert.AreEqual("Player", loaded.Name);
        }

        [Test]
        public void SaveThenLoad_PersistsAcrossInstances()
        {
            _persistenceManager.Save("progress", new TestSaveData { Score = 7, Name = "Reloaded" });

            var reloadedManager = new PersistenceManager(_filePath);
            var loaded = reloadedManager.Load<TestSaveData>("progress");

            Assert.AreEqual(7, loaded.Score);
            Assert.AreEqual("Reloaded", loaded.Name);
        }

        [Test]
        public void MultipleKeys_AreStoredIndependently()
        {
            _persistenceManager.Save("a", new TestSaveData { Score = 1 });
            _persistenceManager.Save("b", new TestSaveData { Score = 2 });

            Assert.AreEqual(1, _persistenceManager.Load<TestSaveData>("a").Score);
            Assert.AreEqual(2, _persistenceManager.Load<TestSaveData>("b").Score);
        }

        [Test]
        public void Delete_RemovesKey()
        {
            _persistenceManager.Save("progress", new TestSaveData { Score = 5 });

            _persistenceManager.Delete("progress");

            Assert.IsFalse(_persistenceManager.Exists("progress"));
        }

        [Test]
        public void Delete_WithMissingKey_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _persistenceManager.Delete("missing"));
        }
    }
}
