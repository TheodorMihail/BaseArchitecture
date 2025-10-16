using BaseArchitecture.Core;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using IFactory = BaseArchitecture.Core.IFactory;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Manages UI screen and HUD lifecycle and presentation.
    /// Supports screens with return values and typed parameters.
    /// HUDs are persistent UI elements that return immediately.
    /// Update the DI container in scene installers to enable UI instantiation in that context.
    /// </summary>
    public interface IUIManager : IInitializable, IDisposable
    {
        void UpdateDIContainer(DiContainer container);

        UniTask ShowScreen<T>() where T : IScreen;
        UniTask<TResult> ShowScreen<T, TResult>()
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult;
        UniTask ShowScreen<T, TParam>(TParam param)
            where T : IScreenWithParams<TParam>
            where TParam : IScreenParam;
        UniTask<TResult> ShowScreen<T, TParam, TResult>(TParam param)
            where T : IScreenWithParams<TParam>, IScreenWithResult<TResult>
            where TParam : IScreenParam
            where TResult : IScreenResult;

        T ShowHUD<T>() where T : IHUD;
    }

    public class UIManager : IUIManager
    {
        private readonly IFactory _factory;
        private DiContainer _diContainer;

        public UIManager(IFactory factory)
        {
            _factory = factory;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        public void UpdateDIContainer(DiContainer container)
        {
            _diContainer = container;
        }

        public async UniTask ShowScreen<T>() where T : IScreen
        {
            await ShowScreenInternal<T>();
        }

        public async UniTask<TResult> ShowScreen<T, TResult>()
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult
        {
            return await ShowScreenInternal<T, TResult>();
        }

        public async UniTask<TResult> ShowScreen<T, TParam, TResult>(TParam param)
            where T : IScreenWithParams<TParam>, IScreenWithResult<TResult>
            where TParam : IScreenParam
            where TResult : IScreenResult   
        {
            return await ShowScreenInternal<T, TParam, TResult>(param);
        }
        
        public async UniTask ShowScreen<T, TParam>(TParam param)
            where T : IScreenWithParams<TParam>
            where TParam : IScreenParam
        {
            await ShowScreenInternal<T, TParam>(param);
        }

        private async UniTask ShowScreenInternal<T>() where T : IScreen
        {
            var screen = CreateScreenInstance<T>();
            await WaitForScreenClosure(screen);
        }

        private async UniTask<TResult> ShowScreenInternal<T, TResult>()
            where T : IScreenWithResult<TResult>
            where TResult : IScreenResult
        {
            var screen = CreateScreenInstance<T>();
            await WaitForScreenClosure(screen);
            return screen.GetResult();
        }

        private async UniTask ShowScreenInternal<T, TParam>(TParam param)
            where T : IScreenWithParams<TParam>
            where TParam : IScreenParam
        {
            var screen = CreateScreenInstance<T>();
            screen.SetParameter(param);
            await WaitForScreenClosure(screen);
        }

        private async UniTask<TResult> ShowScreenInternal<T, TParam, TResult>(TParam param)
            where T : IScreenWithParams<TParam>, IScreenWithResult<TResult>
            where TParam : IScreenParam
            where TResult : IScreenResult
        {
            var screen = CreateScreenInstance<T>();
            screen.SetParameter(param);
            await WaitForScreenClosure(screen);
            return screen.GetResult();
        }

        private async UniTask WaitForScreenClosure<T>(T screen) where T : IScreen
        {
            screen.Initialize();
            await screen.WaitForClosure();
        }

        private T CreateUIComponentInstance<T>() where T : IUIComponent
        {
            string containerID;

            if (typeof(IScreen).IsAssignableFrom(typeof(T)))
            {
                containerID = IScreen.ScreensContainerID;
            }
            else if (typeof(IHUD).IsAssignableFrom(typeof(T)))
            {
                containerID = IHUD.HUDContainerID;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported UI component type: {typeof(T).Name}");
            }

            var container = _diContainer.TryResolveId<Transform>(containerID);
            T uiComponent = _factory.CreateNewObject<T>(container);
            return uiComponent;
        }

        private T CreateScreenInstance<T>() where T : IScreen
        {
            return CreateUIComponentInstance<T>();
        }

        public T ShowHUD<T>() where T : IHUD
        {
            T hud = CreateUIComponentInstance<T>();
            hud.Initialize();
            return hud;
        }
    }
}