using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Manages scene loading with async operations and lifecycle events.
    /// Subscribe to OnSceneLoadStarted/OnSceneLoaded to react to scene transitions.
    /// </summary>
    public interface IScenesManager : IInitializable, IDisposable
    {
        event Action<string> OnSceneLoadStarted;
        event Action<string> OnSceneLoaded;
        void LoadScene(string sceneLoad);
        void ReloadCurrentScene();
    }

    public class ScenesManager : IScenesManager
    {
        public event Action<string> OnSceneLoaded;
        public event Action<string> OnSceneLoadStarted;

        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty!");
                return;
            }

            Debug.Log($"Load Scene: {sceneName}");
            OnSceneLoadStarted?.Invoke(sceneName);

            var token = _cancellationTokenSource.Token;
            var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            asyncOperation.completed += (obj) =>
            {
                if (token.IsCancellationRequested)
                    return;

                Debug.Log($"Load Scene: {sceneName} - Scene loaded Finished!");
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                OnSceneLoaded?.Invoke(scene.name);
            };
        }

        public void ReloadCurrentScene()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            LoadScene(scene.name);
        }
    }
}