using NUnit.Framework;
using BaseArchitecture.Core;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using Zenject;

namespace BaseArchitecture.Tests
{
    public class TestPoolableObject : MonoBehaviour, IPoolableObject
    {
        public int SpawnCount { get; private set; }
        public int DespawnCount { get; private set; }

        public void OnSpawned()
        {
            SpawnCount++;
        }

        public void OnDespawned()
        {
            DespawnCount++;
        }
    }

    [TestFixture]
    public class ObjectPoolingPlayModeTests : ZenjectUnitTestFixture
    {
        private ObjectPooling _objectPooling;
        private CustomFactory _factory;
        private GameObject _testPrefab;
        private Transform _poolContainer;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _factory = new CustomFactory();
            _factory.UpdateDIContainer(Container);

            _poolContainer = new GameObject("PoolContainer").transform;
            Container.Bind<Transform>().FromInstance(_poolContainer).WhenInjectedInto<ObjectPooling>();
            Container.Bind<ICustomFactory>().FromInstance(_factory);

            _objectPooling = Container.Instantiate<ObjectPooling>();

            _testPrefab = new GameObject("TestPrefab");
            _testPrefab.AddComponent<TestPoolableObject>();
        }

        [TearDown]
        public override void Teardown()
        {
            _objectPooling?.Dispose();

            if (_testPrefab != null)
                Object.DestroyImmediate(_testPrefab);

            if (_poolContainer != null)
                Object.DestroyImmediate(_poolContainer.gameObject);

            base.Teardown();
        }

        [UnityTest]
        public IEnumerator Get_FirstTime_CreatesNewObject()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj = _objectPooling.Get(prefabComponent);

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.SpawnCount);
            Assert.AreEqual(0, obj.DespawnCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_AfterReturn_ReusesObject()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj1 = _objectPooling.Get(prefabComponent);
            _objectPooling.Return(obj1);

            yield return null;

            var obj2 = _objectPooling.Get(prefabComponent);

            Assert.AreSame(obj1, obj2);
            Assert.AreEqual(2, obj2.SpawnCount);
            Assert.AreEqual(1, obj2.DespawnCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Return_CallsOnDespawned()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj = _objectPooling.Get(prefabComponent);
            _objectPooling.Return(obj);

            Assert.AreEqual(1, obj.DespawnCount);
            Assert.IsFalse(obj.gameObject.activeSelf);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Return_MovesToPoolContainer()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj = _objectPooling.Get(prefabComponent);
            _objectPooling.Return(obj);

            Assert.AreEqual(_poolContainer, obj.transform.parent);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Prewarm_CreatesSpecifiedCount()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            _objectPooling.Prewarm(prefabComponent, 3);

            Assert.AreEqual(3, _poolContainer.childCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Prewarm_ObjectsAreAvailableForReuse()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            _objectPooling.Prewarm(prefabComponent, 3);

            yield return null;

            var obj1 = _objectPooling.Get(prefabComponent);
            var obj2 = _objectPooling.Get(prefabComponent);
            var obj3 = _objectPooling.Get(prefabComponent);

            Assert.AreEqual(2, obj1.SpawnCount);
            Assert.AreEqual(2, obj2.SpawnCount);
            Assert.AreEqual(2, obj3.SpawnCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Get_WithParent_SetsParentCorrectly()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();
            var customParent = new GameObject("CustomParent").transform;

            var obj = _objectPooling.Get(prefabComponent, customParent);

            Assert.AreEqual(customParent, obj.transform.parent);

            yield return null;

            Object.DestroyImmediate(customParent.gameObject);
        }

        [UnityTest]
        public IEnumerator ClearPool_RemovesAllPooledObjects()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj1 = _objectPooling.Get(prefabComponent);
            var obj2 = _objectPooling.Get(prefabComponent);
            _objectPooling.Return(obj1);
            _objectPooling.Return(obj2);

            yield return null;

            _objectPooling.ClearPool(_testPrefab.name);

            yield return null;

            Assert.AreEqual(0, _poolContainer.childCount);
        }

        [UnityTest]
        public IEnumerator ClearAll_RemovesAllPools()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            _objectPooling.Prewarm(prefabComponent, 3);

            yield return null;

            _objectPooling.ClearAll();

            yield return null;

            Assert.AreEqual(0, _poolContainer.childCount);
        }

        [UnityTest]
        public IEnumerator Return_WithNull_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _objectPooling.Return<TestPoolableObject>(null));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Return_WithInactiveObject_DoesNotThrow()
        {
            var prefabComponent = _testPrefab.GetComponent<TestPoolableObject>();

            var obj = _objectPooling.Get(prefabComponent);
            _objectPooling.Return(obj);
            _objectPooling.Return(obj);

            yield return null;
        }
    }
}
