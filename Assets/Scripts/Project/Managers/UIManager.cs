using Base.Systems;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using IFactory = Base.Systems.IFactory;

namespace Base.Project
{
    public interface IUIManager : IInitializable, IDisposable
    {
        void UpdateDIContainer(DiContainer container);
        UniTask ShowScreen<T>() where T : IScreen;
        UniTask ShowScreen<T>(params object[] parameters) where T : IScreen;

        UniTask<TResult> ShowScreen<T, TResult>()
            where T : IScreen<TResult>
            where TResult : IScreenResult;
        UniTask<TResult> ShowScreen<T, TResult>(params object[] parameters)
            where T : IScreen<TResult>
            where TResult : IScreenResult;
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

        public async UniTask ShowScreen<T>(params object[] parameters) where T : IScreen
        {
            await ShowScreenInternal<T>(parameters);
        }

        public async UniTask<TResult> ShowScreen<T, TResult>() 
            where T : IScreen<TResult>
            where TResult : IScreenResult
        {
            return await ShowScreenInternal<T, TResult>();
        }

        public async UniTask<TResult> ShowScreen<T, TResult>(params object[] parameters) 
            where T : IScreen<TResult>
            where TResult : IScreenResult
        {
            return await ShowScreenInternal<T, TResult>(parameters);
        }

        private async UniTask ShowScreenInternal<T>(params object[] parameters) where T : IScreen
        {
            await WaitForScreenClosure<T>(parameters);
        }

        private async UniTask<TResult> ShowScreenInternal<T, TResult>(params object[] parameters) 
            where T : IScreen<TResult>
            where TResult : IScreenResult
        {
            var screen = await WaitForScreenClosure<T>(parameters);
            return screen.GetResult();
        }

        private async UniTask<T> WaitForScreenClosure<T>(params object[] parameters) where T : IScreen
        {
            var screensContainer = _diContainer.TryResolveId<Transform>(IScreen.ScreensContainerID);
            T screen = default;

            if (parameters.Length == 0)
            {
                screen = _factory.CreateNewObject<T>(screensContainer);
            }
            else
            {
                screen = _factory.CreateNewObject<T>(screensContainer, parameters);
            }

            screen.Initialize();
            await screen.WaitForClosure();
            screen.Dispose();
            return screen;
        }
    }
}