using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Base.Project
{
    public interface IScenesManager : IInitializable, IDisposable
    {
        event Action<string> OnSceneLoadStarted;
        event Action<string> OnSceneLoaded;
        void LoadScene(string sceneLoad);
        void ReloadCurrentScene();
    }

    public class ScenesManager : IScenesManager
    {
        public enum SceneType
        {
            Preload,
            MainMenu,
            Game
        }

        public event Action<string> OnSceneLoaded;
        public event Action<string> OnSceneLoadStarted;

        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void LoadScene(string sceneLoad)
        {
            if (!Enum.TryParse(sceneLoad, out SceneType sceneType))
                return;

            LoadScene(sceneType);
        }

        public void ReloadCurrentScene()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            LoadScene(scene.name);
        }

        private void LoadScene(SceneType sceneTypeLoad)
        {
            Debug.Log($"Load Scene: {sceneTypeLoad}");
            OnSceneLoadStarted?.Invoke(sceneTypeLoad.ToString());

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneTypeLoad.ToString());
            asyncOperation.WithCancellation(_cancellationTokenSource.Token);

            asyncOperation.completed += (obj) =>
            {
                Debug.Log($"Load Scene: {sceneTypeLoad} Scene loaded Finished!");
                var scene = GetSceneReference(sceneTypeLoad);
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                OnSceneLoaded?.Invoke(scene.name);
            };
        }

        private Scene GetSceneReference(SceneType sceneTypeUnload)
        {
            return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneTypeUnload.ToString());
        }
    }
}