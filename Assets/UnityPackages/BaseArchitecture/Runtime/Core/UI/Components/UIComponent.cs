using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Base interface for all UI components (Screens and HUDs).
    /// Provides common lifecycle management.
    /// </summary>
    public interface IUIComponent : IInitializable, IDisposable
    {
        string UICategoryID { get; }
        string UIContainerID { get; }
        event Action OnClosed;
        void Close();

        /// <summary>
        /// Initializes this component with typed parameters. Applied to the Model,
        /// if the Model implements <see cref="IModelWithParams{TParam}"/> for the matching type.
        /// </summary>
        void Initialize<TParam>(TParam parameter);
    }

    /// <summary>
    /// Base abstract class for all UI components that manages MVC lifecycle.
    /// Handles automatic prefab loading, MVC instantiation, and error handling.
    /// </summary>
    public abstract class UIComponent<M, V, C> : IUIComponent
        where M : IModel
        where V : IView
        where C : IController
    {
        [Inject] protected readonly ICustomFactory _factory;
        [Inject] protected readonly IAddressablesManager _addressablesManager;

        protected M _model;
        protected V _view;
        protected C _controller;

        public abstract string UIContainerID { get; }
        public abstract string UICategoryID { get; }
        public event Action OnClosed;

        protected abstract Transform GetContainer();

        public async void Initialize()
        {
            await CreateMVC();

            if (_controller == null)
                return;

            _controller.Initialize();
        }

        public async void Initialize<TParam>(TParam parameter)
        {
            await CreateMVC();

            if (_model is IModelWithParams<TParam> modelWithParams)
                modelWithParams.InitializeWithParameters(parameter);

            if (_controller == null)
                return;

            _controller.Initialize();
        }

        public virtual void Dispose()
        {
            if (_controller == null)
                return;

            _controller.Dispose();
        }

        public virtual void Close()
        {
            if (_view != null && _view is MonoBehaviour monoBehaviour)
            {
                GameObject.Destroy(monoBehaviour.gameObject);
            }

            Dispose();
            OnClosed?.Invoke();
        }

        /// <summary>
        /// Creates Model, View, and Controller instances with automatic prefab loading.
        /// </summary>
        protected async UniTask CreateMVC()
        {
            try
            {
                var addressablePath = GetAddressablesPath<V>();
                var prefab = await _addressablesManager.LoadPrefab(addressablePath);

                if (prefab == null)
                {
                    OnLoadFailed("Prefab could not be loaded!");
                    return;
                }

                _view = _factory.CreateFromPrefab(prefab, GetContainer()).GetComponent<V>();
                _model = _factory.CreateNewObject<M>();
                _controller = _factory.CreateNewObject<C>(this, _model, _view);
                MVCCreated();
            }
            catch (Exception ex)
            {
                OnLoadFailed(ex.Message);
            }
        }

        /// <summary>
        /// Called when MVC loading fails. Override to provide custom failure handling.
        /// </summary>
        protected virtual void OnLoadFailed(string error)
        {
           this.LogError($"Failed to load {UICategoryID}: {GetType().Name} \n {error}");
            Close();
        }

        /// <summary>
        /// Called after MVC components are created and ready.
        /// Override to perform custom initialization logic.
        /// </summary>
        protected virtual void MVCCreated()
        {
        }

        private string GetAddressablesPath<T>() where T : IView
        {
            var attribute = typeof(T).GetCustomAttribute<AddressablePathAttribute>();
            return attribute?.Path;
        }
    }
}
