using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Generic object pooling system for reusing GameObjects/Components.
    /// Reduces garbage collection and instantiation overhead.
    /// </summary>
    public interface IObjectPooling: IDisposable
    {
        /// <summary>
        /// Gets an instance from the pool or creates a new one if pool is empty.
        /// Automatically calls OnSpawned() on the IPoolableObject instance.
        /// </summary>
        T Get<T>(T prefab, Transform parent = null) where T : MonoBehaviour, IPoolableObject;

        /// <summary>
        /// Returns an instance to the pool for later reuse.
        /// Automatically calls OnDespawned() on the IPoolableObject instance.
        /// </summary>
        void Return<T>(T instance) where T : MonoBehaviour, IPoolableObject;

        /// <summary>
        /// Clears all pools and destroys all pooled objects.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Clears a specific pool by prefab name.
        /// </summary>
        void ClearPool(string poolKey);

        /// <summary>
        /// Pre-warms a pool by creating instances in advance.
        /// </summary>
        void Prewarm<T>(T prefab, int count, Transform parent = null) where T : MonoBehaviour, IPoolableObject;
    }

    public class ObjectPooling : IObjectPooling
    {
        [Inject] private readonly ICustomFactory _factory;
        [Inject] private readonly Transform _poolContainer;

        private readonly Dictionary<string, Queue<MonoBehaviour>> _pools = new Dictionary<string, Queue<MonoBehaviour>>();
        private readonly HashSet<MonoBehaviour> _activeObjects = new HashSet<MonoBehaviour>();
        public void Dispose()
        {
            ClearAll();
        }

        public T Get<T>(T prefab, Transform parent = null) where T : MonoBehaviour, IPoolableObject
        {
            string poolKey = prefab.name;
            T instance;

            if (_pools.TryGetValue(poolKey, out var pool) && pool.Count > 0)
            {
                instance = (T)pool.Dequeue();
                instance.transform.SetParent(parent);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance = _factory.CreateFromPrefab(prefab, parent);
                instance.name = poolKey;
            }

            _activeObjects.Add(instance);
            instance.OnSpawned();
            return instance;
        }

        public void Return<T>(T instance) where T : MonoBehaviour, IPoolableObject
        {
            if (instance == null || !_activeObjects.Contains(instance))
                return;

            _activeObjects.Remove(instance);
            instance.OnDespawned();

            string poolKey = instance.name;

            if (!_pools.ContainsKey(poolKey))
            {
                _pools[poolKey] = new Queue<MonoBehaviour>();
            }

            _pools[poolKey].Enqueue(instance);
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_poolContainer);
        }

        public void ClearAll()
        {
            foreach (var poolKey in new List<string>(_pools.Keys))
            {
                ClearPool(poolKey);
            }

            _activeObjects.Clear();
        }

        public void ClearPool(string poolKey)
        {
            if (_pools.TryGetValue(poolKey, out var pool))
            {
                while (pool.Count > 0)
                {
                    var obj = pool.Dequeue();
                    if (obj != null)
                    {
                        GameObject.DestroyImmediate(obj.gameObject);
                    }
                }

                _pools.Remove(poolKey);
            }
        }

        public void Prewarm<T>(T prefab, int count, Transform parent = null) where T : MonoBehaviour, IPoolableObject
        {
            List<T> tempList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                tempList.Add(Get(prefab, parent));
            }
            
            for (int i = 0; i < count; i++)
            {
                Return(tempList[i]);
            }
        }
    }
}
