using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    public interface ICustomFactory
    {
        void UpdateDIContainer(DiContainer container);

        T CreateNewObject<T>();
        T CreateNewObject<T>(params object[] extraArgs);

        GameObject CreateFromPrefab(GameObject prefab, Transform container);

        T CreateFromPrefab<T>(T prefab, Transform container) where T : MonoBehaviour;
        T CreateFromPrefab<T>(T prefab, Transform container, params object[] extraArgs) where T : MonoBehaviour;
    }

    public class CustomFactory : ICustomFactory
    {
        private DiContainer _container;

        public void UpdateDIContainer(DiContainer container)
        {
            _container = container;
        }

        public T CreateFromPrefab<T>(T prefab, Transform container) where T : MonoBehaviour
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, container);
        }

        public T CreateFromPrefab<T>(T prefab, Transform container, params object[] extraArgs) where T : MonoBehaviour
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, container, extraArgs);
        }

        public GameObject CreateFromPrefab(GameObject prefab, Transform container)
        {
            return _container.InstantiatePrefab(prefab, container);
        }

        public T CreateNewObject<T>()
        {
            return _container.Instantiate<T>();
        }

        public T CreateNewObject<T>(params object[] extraArgs)
        {
            return _container.Instantiate<T>(extraArgs);
        }
    }
}
