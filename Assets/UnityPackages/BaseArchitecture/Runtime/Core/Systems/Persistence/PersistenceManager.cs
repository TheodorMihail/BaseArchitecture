using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Marker interface for types persisted via IPersistenceManager.
    /// </summary>
    public interface ISaveData
    {
    }

    /// <summary>
    /// Save data that carries the version it was written with, so a build can discard data it can no
    /// longer read. Opt-in: plain ISaveData is loaded with Load and is never version checked.
    /// </summary>
    public interface IVersionedSaveData : ISaveData
    {
        /// <summary>The version the stored data was written with. 0 means it predates versioning.</summary>
        int Version { get; set; }
    }

    public interface IPersistenceManager
    {
        bool Exists(string key);
        T Load<T>(string key) where T : class, ISaveData, new();

        /// <summary>
        /// Loads data, replacing it with an empty instance when the stored version differs from the
        /// one this build writes. Each key is versioned on its own, so discarding one leaves the rest.
        /// </summary>
        T LoadVersioned<T>(string key, int currentVersion) where T : class, IVersionedSaveData, new();
        void Save<T>(string key, T data) where T : class, ISaveData;
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

        public T Load<T>(string key) where T : class, ISaveData, new()
        {
            return _entries.TryGetValue(key, out string json) ? JsonConvert.DeserializeObject<T>(json) : new T();
        }

        /// <remarks>A member missing from the stored JSON keeps its default, so data written before
        /// the version field existed loads as version 0. Leave a type's current version at 0 until its
        /// contents stop being usable and such data is still accepted.</remarks>
        public T LoadVersioned<T>(string key, int currentVersion) where T : class, IVersionedSaveData, new()
        {
            T data = Load<T>(key);

            if (data.Version == currentVersion)
            {
                return data;
            }

            // Nothing stored means nothing was discarded, so only a real replacement is reported.
            if (Exists(key))
            {
                data.LogWarning($"Discarding save data written by version {data.Version}, this build expects {currentVersion}.");
            }

            var replacement = new T { Version = currentVersion };
            Save(key, replacement);

            return replacement;
        }

        public void Save<T>(string key, T data) where T : class, ISaveData
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
