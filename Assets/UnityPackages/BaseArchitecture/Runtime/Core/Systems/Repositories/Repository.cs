using System;
using System.Collections.Generic;

namespace BaseArchitecture.Core
{
    public interface IRepository
    {
        void AddObjects<T>(IEnumerable<T> objs) where T : IRepositoryObject;
        void AddObject<T>(T obj) where T : IRepositoryObject;
        void RemoveObject<T>(T obj) where T : IRepositoryObject;
        T Get<T>(string id) where T : IRepositoryObject;
        IReadOnlyList<T> GetAll<T>() where T : IRepositoryObject;
    }

    public class Repository : IRepository
    {
        private readonly Dictionary<Type, Dictionary<string, IRepositoryObject>> _buckets = new();

        public void AddObjects<T>(IEnumerable<T> objs) where T : IRepositoryObject
        {
            foreach (var obj in objs)
                AddObject(obj);
        }

        public void AddObject<T>(T obj) where T : IRepositoryObject
        {
            var type = typeof(T);
            if (!_buckets.TryGetValue(type, out var bucket))
            {
                bucket = new Dictionary<string, IRepositoryObject>();
                _buckets[type] = bucket;
            }

            if (bucket.ContainsKey(obj.ObjectID))
            {
                this.LogWarning($"Object with ID '{obj.ObjectID}' already exists in bucket {type.Name}. Ignoring duplicate.");
                return;
            }

            bucket.Add(obj.ObjectID, obj);
        }

        public void RemoveObject<T>(T obj) where T : IRepositoryObject
        {
            if (_buckets.TryGetValue(typeof(T), out var bucket))
                bucket.Remove(obj.ObjectID);
        }

        public T Get<T>(string id) where T : IRepositoryObject
        {
            var type = typeof(T);
            if (!_buckets.TryGetValue(type, out var bucket) || !bucket.TryGetValue(id, out var obj))
            {
                this.LogError($"Object with ID '{id}' not found in bucket {type.Name}.");
                return default;
            }

            return (T)obj;
        }

        public IReadOnlyList<T> GetAll<T>() where T : IRepositoryObject
        {
            if (!_buckets.TryGetValue(typeof(T), out var bucket))
                return Array.Empty<T>();

            var result = new List<T>(bucket.Count);
            foreach (var obj in bucket.Values)
                result.Add((T)obj);
                
            return result;
        }
    }
}
