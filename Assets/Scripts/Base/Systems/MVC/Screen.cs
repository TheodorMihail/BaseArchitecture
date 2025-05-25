using Base.Project;
using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Base.Systems
{
    public interface IScreen : IInitializable, IDisposable
    {
        const string ScreensContainerID = "ScreenContainer";

        UniTask WaitForClosure();
        void CloseScreen();
    }

    public interface IScreen<TResult> : IScreen
        where TResult : IScreenResult
    {
        TResult GetResult();
        void SetResult(TResult result);
    }

    public interface IScreenResult { }

    public abstract class Screen<M, V, C> : IScreen
        where M : IModel
        where V : IView
        where C : IController
    {
        [Inject] private readonly IFactory _factory;
        [Inject] private readonly IAddressablesManager _addressablesManager;
        [Inject] private readonly Transform _screenContainer;

        protected M _model;
        protected V _view;
        protected C _controller;

        private UniTaskCompletionSource _screenClosedTcs = new UniTaskCompletionSource();

        public async void Initialize()
        {
            await CreateMVC();

            if (_controller == null)
                return;

            _controller.Initialize();
        }

        public void Dispose()
        {
            if (_controller == null)
                return;

            _controller.Dispose();
        }

        public UniTask WaitForClosure()
        {
            return _screenClosedTcs.Task;
        }

        public void CloseScreen()
        {
            _screenClosedTcs.TrySetResult();
        }

        protected async UniTask CreateMVC()
        {
            var addressablePath = GetAddressablesPath<V>();
            var prefab = await _addressablesManager.LoadPrefab(addressablePath);

            if (prefab == null)
            {
                CloseScreen();
                return;
            }

            _view = _factory.CreateFromPrefab(prefab, _screenContainer).GetComponent<V>();
            _model = _factory.CreateNewObject<M>();
            _controller = _factory.CreateNewObject<C>(this, _model, _view);
        }

        private string GetAddressablesPath<T>() where T : IView
        {
            var attribute = typeof(T).GetCustomAttribute<AddressablePathAttribute>();
            return attribute?.Path;
        }
    }

    public abstract class Screen<M, V, C, TResult> : Screen<M, V, C>, IScreen<TResult>
        where M : IModel
        where V : IView
        where C : IController
        where TResult : IScreenResult
    {
        protected TResult _result;

        public TResult GetResult()
        {
            return _result;
        }

        public void SetResult(TResult result)
        {
            _result = result;
        }
    }
}
