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

            var retrieved1 = _repository.Get<TestRepositoryObject>("obj1");
            var retrieved2 = _repository.Get<TestRepositoryObject>("obj2");

            Assert.IsNotNull(retrieved1);
            Assert.IsNotNull(retrieved2);
            Assert.AreEqual("Object 1", retrieved1.Name);
            Assert.AreEqual("Object 2", retrieved2.Name);
        }

        [Test]
        public void GetObject_WithValidID_ReturnsCorrectObject()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);

            var retrieved = _repository.Get<TestRepositoryObject>("test1");

            Assert.IsNotNull(retrieved);
            Assert.AreEqual("test1", retrieved.ID);
            Assert.AreEqual("Test Object", retrieved.Name);
        }

        [Test]
        public void GetObject_WithInvalidID_ReturnsNullAndLogsError()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);

            _repository.Get<TestRepositoryObject>("nonexistent");
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'nonexistent' not found.");
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

            var retrieved = _repository.Get<TestRepositoryObject>("duplicate");

            Assert.IsNotNull(retrieved);
            Assert.AreEqual("First", retrieved.Name);
        }

        [Test]
        public void GetObject_WithEmptyRepository_ReturnsNullAndLogsError()
        {
            Assert.IsNull(_repository.Get<TestRepositoryObject>("any"));
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'any' not found.");
        }

        [Test]
        public void GetObjectGeneric_WithValidID_ReturnsTypedObject()
        {
            var testObject = new TestRepositoryObject { ID = "typed1", Name = "Typed Object" };
            _repository.AddObject(testObject);

            var retrieved = _repository.Get<TestRepositoryObject>("typed1");

            Assert.IsNotNull(retrieved);
            Assert.AreEqual("typed1", retrieved.ID);
            Assert.AreEqual("Typed Object", retrieved.Name);
        }

        [Test]
        public void GetObjectGeneric_WithInvalidID_ReturnsDefaultAndLogsError()
        {
            _repository.Get<TestRepositoryObject>("nonexistent");
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'nonexistent' not found.");
        }


        [Test]
        public void GetObjectGeneric_WithInvalidType_ReturnsDefaultAndLogsError()
        {
            var testObject = new TestRepositoryObject { ID = "test1", Name = "Test Object" };
            _repository.AddObject(testObject);
            var testObject2 = new TestRepositoryObject2 { ID = "test2" };
            _repository.AddObject(testObject2);

            _repository.Get<TestRepositoryObject2>("test1");
            LogAssert.Expect(LogType.Error, "[Repository] [Error] Object with ID 'test1' is not of type TestRepositoryObject2, it is TestRepositoryObject.");
        }

        [Test]
        public void AddObjects_WithEmptyList_DoesNotThrow()
        {
            var emptyList = new List<TestRepositoryObject>();
            Assert.DoesNotThrow(() => _repository.AddObjects(emptyList));
        }
    }
}
