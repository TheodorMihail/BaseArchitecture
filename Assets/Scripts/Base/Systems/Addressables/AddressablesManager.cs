using Base.Systems;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Base.Systems
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
        private readonly IErrorManager _errorManager;

        public AddressablesManager(IErrorManager errorManager)
        {
            _errorManager = errorManager;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public async UniTask<GameObject> LoadPrefab(string addressablePath)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(addressablePath);
                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    _errorManager.LogError($"Failed to load prefab: {addressablePath}");
                    return null;
                }

                return handle.Result;
            }
            catch (Exception ex)
            {
                _errorManager.LogError($"Exception loading prefab: {addressablePath}", ex);
                return null;
            }
        }
    }
}