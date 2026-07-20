using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace BaseArchitecture.Core
{
    public interface IPersistenceManager
    {
        bool Exists(string key);
        T Load<T>(string key) where T : class, new();
        void Save<T>(string key, T data) where T : class;
        void Delete(string key);
    }

    /// <summary>
    /// Default local implementation: all keys are stored together in a single JSON file on disk.
    /// A networked/backend implementation can be added later as a separate class implementing IPersistenceManager.
    /// </summary>
    public class PersistenceManager : IPersistenceManager
    {
        private readonly string _filePath;
        private readonly Dictionary<string, string> _entries;

        public PersistenceManager(string filePath = null)
        {
            _filePath = filePath ?? Path.Combine(Application.persistentDataPath, "SaveData.json");
            _entries = File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_filePath)) ?? new Dictionary<string, string>()
                : new Dictionary<string, string>();
        }

        public bool Exists(string key) => _entries.ContainsKey(key);

        public T Load<T>(string key) where T : class, new()
        {
            return _entries.TryGetValue(key, out string json) ? JsonConvert.DeserializeObject<T>(json) : new T();
        }

        public void Save<T>(string key, T data) where T : class
        {
            _entries[key] = JsonConvert.SerializeObject(data);
            Flush();
        }

        public void Delete(string key)
        {
            if (_entries.Remove(key))
                Flush();
        }

        private void Flush()
        {
            string directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_entries));
        }
    }
}
