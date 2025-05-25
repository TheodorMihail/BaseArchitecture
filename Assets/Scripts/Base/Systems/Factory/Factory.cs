using UnityEngine;
using Zenject;

namespace Base.Systems
{
    public interface IFactory
    {
        T CreateNewObject<T>();
        T CreateNewObject<T>(params object[] extraArgs);

        GameObject CreateFromPrefab(GameObject prefab, Transform container);

        T CreateFromPrefab<T>(T prefab, Transform container);
        T CreateFromPrefab<T>(T prefab, Transform container, params object[] extraArgs);
    }

    public class Factory : IFactory
    {
        protected readonly DiContainer _container;

        public Factory(DiContainer container)
        {
            _container = container;
        }

        public T CreateFromPrefab<T>(T prefab, Transform container)
        {
            return _container.InstantiatePrefabForComponent<T>((prefab as MonoBehaviour), container);
        }

        public T CreateFromPrefab<T>(T prefab, Transform container, params object[] extraArgs)
        {
            return _container.InstantiatePrefabForComponent<T>((prefab as MonoBehaviour), container, extraArgs);
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
