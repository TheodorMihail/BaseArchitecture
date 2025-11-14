using NUnit.Framework;
using BaseArchitecture.Core;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using Zenject;

namespace BaseArchitecture.Tests
{
    public class TestMonoBehaviour : MonoBehaviour
    {
        public string TestValue { get; set; }
    }

    public class TestMonoBehaviourWithDependency : MonoBehaviour
    {
        [Inject] public TestSimpleClass Dependency { get; private set; }
    }

    public class TestSimpleClass
    {
        public string Value { get; set; } = "injected";
    }

    [TestFixture]
    public class CustomFactoryPlayModeTests : ZenjectUnitTestFixture
    {
        private CustomFactory _factory;
        private GameObject _testPrefab;
        private Transform _testParent;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _factory = new CustomFactory();

            // Setup Zenject container
            Container.Bind<TestSimpleClass>().AsSingle();
            _factory.UpdateDIContainer(Container);

            // Create test parent
            _testParent = new GameObject("TestParent").transform;

            // Create test prefab
            _testPrefab = new GameObject("TestPrefab");
            _testPrefab.AddComponent<TestMonoBehaviour>();
        }

        [TearDown]
        public override void Teardown()
        {
            if (_testPrefab != null)
                Object.DestroyImmediate(_testPrefab);

            if (_testParent != null)
                Object.DestroyImmediate(_testParent.gameObject);
                
            base.Teardown();
        }

        [UnityTest]
        public IEnumerator CreateFromPrefab_GameObject_CreatesInstance()
        {
            var instance = _factory.CreateFromPrefab(_testPrefab, _testParent);

            Assert.IsNotNull(instance);
            Assert.AreEqual(_testParent, instance.transform.parent);
            Assert.AreNotSame(_testPrefab, instance);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CreateFromPrefab_MonoBehaviour_CreatesAndReturnsComponent()
        {
            var prefabComponent = _testPrefab.GetComponent<TestMonoBehaviour>();

            var instance = _factory.CreateFromPrefab(prefabComponent, _testParent);

            Assert.IsNotNull(instance);
            Assert.AreEqual(_testParent, instance.transform.parent);
            Assert.AreNotSame(prefabComponent, instance);
            Assert.IsInstanceOf<TestMonoBehaviour>(instance);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CreateFromPrefab_WithNullParent_CreatesInstanceWithoutParent()
        {
            var instance = _factory.CreateFromPrefab(_testPrefab, null);

            Assert.IsNotNull(instance);
            Assert.IsNull(instance.transform.parent);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CreateFromPrefab_WithDependencyInjection_InjectsDependencies()
        {
            var prefabGameObjectWithDI = new GameObject("PrefabWithDI");
            var prefabTypeWithDI = prefabGameObjectWithDI.AddComponent<TestMonoBehaviourWithDependency>();

            var instance = _factory.CreateFromPrefab(prefabTypeWithDI, _testParent);

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Dependency);
            Assert.AreEqual("injected", instance.Dependency.Value);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CreateFromPrefab_MultipleCalls_CreatesMultipleInstances()
        {
            var instance1 = _factory.CreateFromPrefab(_testPrefab, _testParent);
            var instance2 = _factory.CreateFromPrefab(_testPrefab, _testParent);

            Assert.IsNotNull(instance1);
            Assert.IsNotNull(instance2);
            Assert.AreNotSame(instance1, instance2);
            Assert.AreEqual(2, _testParent.childCount);

            yield return null;
        }

        [Test]
        public void CreateNewObject_SimpleClass_CreatesInstance()
        {
            var instance = _factory.CreateNewObject<TestSimpleClass>();

            Assert.IsNotNull(instance);
            Assert.AreEqual("injected", instance.Value);
        }
    }
}
