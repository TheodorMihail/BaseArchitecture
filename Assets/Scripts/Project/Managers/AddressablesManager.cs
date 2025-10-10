using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Base.Project
{
    /// <summary>
    /// Wrapper for Unity Addressables system. Loads prefabs asynchronously by their addressable path.
    /// Returns null if loading fails - check logs for error details.
    /// </summary>
    public interface IAddressablesManager : IInitializable, IDisposable
    {
        UniTask<GameObject> LoadPrefab(string addressablePath);
    }

    public class AddressablesManager : IAddressablesManager
    {
        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public async UniTask<GameObject> LoadPrefab(string addressablePath)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(addressablePath);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load prefab: {addressablePath}");
                return default;
            }

            return handle.Result;
        }
    }
}