using System.Collections.Generic;

namespace BaseArchitecture.Core
{
    public interface IRepository
    {
        void AddObjects(IEnumerable<IRepositoryObject> objs);
        void AddObject(IRepositoryObject obj);
        void RemoveObject(IRepositoryObject obj);
        IRepositoryObject GetObject(string id);
        T GetObject<T>(string id) where T : IRepositoryObject;
    }

    public class Repository : IRepository
    {
        private readonly Dictionary<string, IRepositoryObject> _objects = new();

        public void AddObjects(IEnumerable<IRepositoryObject> objs)
        {
            foreach (var obj in objs)
            {
                AddObject(obj);
            }
        }

        public void AddObject(IRepositoryObject obj)
        {
            if (!_objects.ContainsKey(obj.ObjectID))
            {
                _objects.Add(obj.ObjectID, obj);
            }
        }

        public void RemoveObject(IRepositoryObject obj)
        {
            if (_objects.ContainsKey(obj.ObjectID))
            {
                _objects.Remove(obj.ObjectID);
            }
        }

        public IRepositoryObject GetObject(string id)
        {
            if(!_objects.TryGetValue(id, out var obj))
            {
                this.LogError($"Object with ID '{id}' not found.");
            }

            return obj;
        }

        public T GetObject<T>(string id) where T : IRepositoryObject
        {
            var obj = GetObject(id);

            if (obj is T typedObj)
            {
                return typedObj;
            }

            this.LogError($"Object with ID '{id}' cannot be cast to type {typeof(T).Name}.");
            return default;
        }
    }
}
