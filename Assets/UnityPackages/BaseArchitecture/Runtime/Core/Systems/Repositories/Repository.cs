using System;
using System.Collections.Generic;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// In-memory store for objects addressed by ObjectID. Objects are grouped into buckets keyed by
    /// the generic argument they were added with, not by their runtime type.
    /// </summary>
    public interface IRepository
    {
        void AddObjects<T>(IEnumerable<T> objs) where T : IRepositoryObject;

        /// <summary>Duplicate ObjectIDs are ignored with a warning, the first object added wins.</summary>
        void AddObject<T>(T obj) where T : IRepositoryObject;

        void RemoveObject<T>(T obj) where T : IRepositoryObject;

        /// <summary>Logs an error when the id is not present in the bucket.</summary>
        bool TryGet<T>(string id, out T obj) where T : IRepositoryObject;

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

        public bool TryGet<T>(string id, out T obj) where T : IRepositoryObject
        {
            var type = typeof(T);
            obj = default;

            if (id == null)
            {
                this.LogError($"Cannot look up an object in bucket {type.Name}: id was null.");
                return false;
            }

            if (!_buckets.TryGetValue(type, out var bucket) || !bucket.TryGetValue(id, out var found))
            {
                this.LogError($"Object with ID '{id}' not found in bucket {type.Name}.");
                return false;
            }

            obj = (T)found;
            return true;
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
