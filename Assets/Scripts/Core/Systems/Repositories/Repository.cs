using System.Collections.Generic;

namespace BaseArchitecture.Core
{
    public interface IRepository
    {
        void AddObjects(IEnumerable<IRepositoryObject> objs);
        void AddObject(IRepositoryObject obj);
        void RemoveObject(IRepositoryObject obj);
        IRepositoryObject GetObject(string id);
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
            _objects.TryGetValue(id, out var obj);
            return obj;
        }
    }
}
