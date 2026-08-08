using NUnit.Framework;
using BaseArchitecture.Core;
using System.Collections.Generic;
using UnityEngine.TestTools;
using UnityEngine;

namespace BaseArchitecture.Tests
{
    public class TestRepositoryObject : IRepositoryObject
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string ObjectID => ID;
    }

    public class TestRepositoryObject2 : IRepositoryObject
    {
        public string ID { get; set; }
        public string ObjectID => ID;
    }

    [TestFixture]
    public class RepositoryTests
    {
        private Repository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new Repository();
        }

        [Test]
        public void AddObjects_WithValidList_AddsAllObjects()
        {
            var objects = new List<TestRepositoryObject>
            {
                new TestRepositoryObject { ID = "obj1", Name = "Object 1" },
                new TestRepositoryObject { ID = "obj2", Name = "Object 2" }
            };

            _repository.AddObjects(objects);

            _repository.TryGet<TestRepositoryObject>("obj1", out var retrieved1);
            _repository.TryGet<TestRepositoryObject>("obj2", out var retrieved2);

            Assert.IsNotNull(retrieved1);
            Assert.IsNotNull(retrieved2);
            Assert.AreEqual("Object 1", retrieved1.Name);
            Assert.AreEqual("Object 2", retrieved2.Name);
        }

        [Test]
        public void TryGetObject_WithValidID_ReturnsTrueAndCorrectObject()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);

            bool found = _repository.TryGet<TestRepositoryObject>("test1", out var retrieved);

            Assert.IsTrue(found);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("test1", retrieved.ID);
            Assert.AreEqual("Test Object", retrieved.Name);
        }

        [Test]
        public void TryGetObject_WithInvalidID_ReturnsFalseAndLogsError()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);

            bool found = _repository.TryGet<TestRepositoryObject>("nonexistent", out var retrieved);

            Assert.IsFalse(found);
            Assert.IsNull(retrieved);
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'nonexistent' not found in bucket TestRepositoryObject.");
        }

        [Test]
        public void TryGetObject_WithNullID_ReturnsFalseAndLogsNullSpecificError()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);

            bool found = _repository.TryGet<TestRepositoryObject>(null, out var retrieved);

            Assert.IsFalse(found);
            Assert.IsNull(retrieved);
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Cannot look up an object in bucket TestRepositoryObject: id was null.");
        }

        [Test]
        public void AddObjects_WithDuplicateIDs_KeepsFirstObject()
        {
            var objects = new List<TestRepositoryObject>
            {
                new TestRepositoryObject { ID = "duplicate", Name = "First" },
                new TestRepositoryObject { ID = "duplicate", Name = "Second" }
            };
            _repository.AddObjects(objects);

            _repository.TryGet<TestRepositoryObject>("duplicate", out var retrieved);

            Assert.IsNotNull(retrieved);
            Assert.AreEqual("First", retrieved.Name);
        }

        [Test]
        public void TryGetObject_WithEmptyRepository_ReturnsFalseAndLogsError()
        {
            bool found = _repository.TryGet<TestRepositoryObject>("any", out var retrieved);

            Assert.IsFalse(found);
            Assert.IsNull(retrieved);
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'any' not found in bucket TestRepositoryObject.");
        }

        [Test]
        public void TryGetObjectGeneric_WithValidID_ReturnsTrueAndTypedObject()
        {
            var testObject = new TestRepositoryObject { ID = "typed1", Name = "Typed Object" };
            _repository.AddObject(testObject);

            bool found = _repository.TryGet<TestRepositoryObject>("typed1", out var retrieved);

            Assert.IsTrue(found);
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("typed1", retrieved.ID);
            Assert.AreEqual("Typed Object", retrieved.Name);
        }

        [Test]
        public void TryGetObjectGeneric_WithInvalidID_ReturnsFalseAndLogsError()
        {
            bool found = _repository.TryGet<TestRepositoryObject>("nonexistent", out var retrieved);

            Assert.IsFalse(found);
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'nonexistent' not found in bucket TestRepositoryObject.");
        }


        [Test]
        public void TryGetObjectGeneric_WithInvalidType_ReturnsFalseAndLogsError()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);
            var testObject2 = new TestRepositoryObject2 { ID = "test2" };
            _repository.AddObject(testObject2);

            bool found = _repository.TryGet<TestRepositoryObject2>("test1", out var retrieved);

            Assert.IsFalse(found);
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'test1' not found in bucket TestRepositoryObject2.");
        }

        [Test]
        public void AddObjects_WithEmptyList_DoesNotThrow()
        {
            var emptyList = new List<TestRepositoryObject>();
            Assert.DoesNotThrow(() => _repository.AddObjects(emptyList));
        }
    }
}
