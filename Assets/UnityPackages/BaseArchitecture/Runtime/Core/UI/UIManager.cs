using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    public enum UIDisplayTypes
    {
        Queue,     // Wait for existing active UIComponent to close
        Parallel, // Allow multiple active UIComponents in category
        Replace,  // Replace existing active UIComponent in category
    }

    /// <summary>
    /// Manages UI screen and HUD lifecycle and presentation.
    /// Supports screens with return values and typed parameters.
    /// HUDs are persistent UI elements that return immediately.
    /// Update the DI container in scene installers to enable UI instantiation in that context.
    /// </summary>
    public interface IUIManager : IInitializable, IDisposable
    {
        void UpdateDIContainer(DiContainer container);

        void ShowHUD<T>(UIDisplayTypes showType = UIDisplayTypes.Parallel) where T : IHUD;
        void ShowHUD<T, TParam>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Parallel) where T : IHUD;

        UniTask ShowScreen<T>(UIDisplayTypes showType = UIDisplayTypes.Queue) where T : IScreen;
        UniTask<TResult> ShowScreen<T, TResult>(UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult;
        UniTask ShowScreen<T, TParam>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreen;
        UniTask<TResult> ShowScreen<T, TParam, TResult>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult;
    }

    public class UIManager : IUIManager
    {
        [Inject] private readonly ICustomFactory _factory;

        private DiContainer _diContainer;
        private Dictionary<string, List<IUIComponent>> _activeUIComponents;
        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _activeUIComponents = new Dictionary<string, List<IUIComponent>>();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.CancelAndDispose();

            foreach (var uiComponentList in _activeUIComponents.Values)
            {
                foreach (var uiComponent in uiComponentList)
                {
                    uiComponent.Dispose();
                }
            }
            
            _activeUIComponents.Clear();
        }

        public void UpdateDIContainer(DiContainer container)
        {
            _diContainer = container;
        }

        public async void ShowHUD<T>(UIDisplayTypes showType = UIDisplayTypes.Parallel) where T : IHUD
        {
            T hud = await CreateUIComponentInstance<T>(showType);
            hud.Initialize();
        }

        public async void ShowHUD<T, TParam>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Parallel) where T : IHUD
        {
            T hud = await CreateUIComponentInstance<T>(showType);
            hud.Initialize(param);
        }

        #region Screens

        public async UniTask ShowScreen<T>(UIDisplayTypes showType = UIDisplayTypes.Queue) where T : IScreen
        {
            var screen = await CreateUIComponentInstance<T>(showType);
            screen.Initialize();

            await screen.WaitForClosure();
        }

        public async UniTask<TResult> ShowScreen<T, TResult>(UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult
        {
            var screen = await CreateUIComponentInstance<T>(showType);
            screen.Initialize();

            await screen.WaitForClosure();
            return screen.GetResult();
        }

        public async UniTask ShowScreen<T, TParam>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreen
        {
            var screen = await CreateUIComponentInstance<T>(showType);
            screen.Initialize(param);

            await screen.WaitForClosure();
        }

        public async UniTask<TResult> ShowScreen<T, TParam, TResult>(TParam param, UIDisplayTypes showType = UIDisplayTypes.Queue)
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult
        {
            var screen = await CreateUIComponentInstance<T>(showType);
            screen.Initialize(param);

            await screen.WaitForClosure();
            return screen.GetResult();
        }

        #endregion

        /// <summary>Resolves the container and category for the component type, applies the display
        /// mode against the components already active in that category, then creates the instance.</summary>
        private async UniTask<T> CreateUIComponentInstance<T>(UIDisplayTypes showType) where T : IUIComponent
        {
            string containerID;
            string categoryKey;

            if (typeof(IScreen).IsAssignableFrom(typeof(T)))
            {
                containerID = IScreen.ScreensContainerID;
                categoryKey = IScreen.ScreenCategoryID;
            }
            else if (typeof(IHUD).IsAssignableFrom(typeof(T)))
            {
                containerID = IHUD.HUDContainerID;
                categoryKey = IHUD.HUDCategoryID;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported UI component type: {typeof(T).Name}");
            }

            var container = _diContainer.TryResolveId<Transform>(containerID);

            if (!_activeUIComponents.ContainsKey(categoryKey))
            {
                _activeUIComponents[categoryKey] = new List<IUIComponent>();
            }

            var existingComponents = _activeUIComponents[categoryKey];

            switch (showType)
            {
                case UIDisplayTypes.Replace:
                    foreach (var existing in existingComponents)
                    {
                        existing.Close();
                    }
                    existingComponents.Clear();
                    break;

                case UIDisplayTypes.Parallel:
                    foreach (var component in existingComponents)
                    {
                        if (component is T existing)
                        {
                            this.LogWarning($"UI Component of type {typeof(T).Name} is already active. Returning existing instance.");
                            return existing;
                        }
                    }
                    break;

                case UIDisplayTypes.Queue:
                    try
                    {
                        await UniTask.WaitUntil(() => existingComponents.Count == 0, cancellationToken: _cancellationTokenSource.Token);
                    }
                    catch {}
                    break;
            }

            T uiComponent = _factory.CreateNewObject<T>(container);
            uiComponent.OnClosed += () => OnUIComponentClosed(uiComponent);
            existingComponents.Add(uiComponent);

            return uiComponent;
        }

        private void OnUIComponentClosed(IUIComponent uiComponent)
        {
            string categoryKey = uiComponent.UICategoryID;
            if (_activeUIComponents.ContainsKey(categoryKey))
            {
                _activeUIComponents[categoryKey].Remove(uiComponent);
            }
        }
    }
}