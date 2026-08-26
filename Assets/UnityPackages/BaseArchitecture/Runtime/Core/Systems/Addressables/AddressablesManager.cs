using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Loads prefabs asynchronously by addressable path. Each address is loaded at most once and its
    /// handle is released on Dispose. A failed load returns null and logs the reason.
    /// </summary>
    public interface IAddressablesManager : IInitializable, IDisposable
    {
        UniTask<GameObject> LoadPrefab(string addressablePath);
    }

    public class AddressablesManager : IAddressablesManager
    {
        private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _handles =
            new Dictionary<string, AsyncOperationHandle<GameObject>>();

        public void Initialize()
        {
        }

        public void Dispose()
        {
            foreach (var handle in _handles.Values)
                Addressables.Release(handle);
            _handles.Clear();
        }

        public async UniTask<GameObject> LoadPrefab(string addressablePath)
        {
            if (_handles.TryGetValue(addressablePath, out var cached))
                return cached.Result;

            try
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(addressablePath);
                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    this.LogError($"Failed to load prefab: {addressablePath}");
                    return null;
                }

                _handles[addressablePath] = handle;
                return handle.Result;
            }
            catch (Exception ex)
            {
                this.LogError($"Exception loading prefab: {addressablePath}", ex);
                return null;
            }
        }
    }
}