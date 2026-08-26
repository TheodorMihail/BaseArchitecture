using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Shared lifecycle for Screens and HUDs.
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
    /// Base class for Screens and HUDs: loads the View's prefab, builds the MVC trio and owns their
    /// lifecycle.
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
        /// Loads the View's prefab and creates the Model, View and Controller.
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
        /// Called when the MVC trio could not be created.
        /// </summary>
        protected virtual void OnLoadFailed(string error)
        {
           this.LogError($"Failed to load {UICategoryID}: {GetType().Name} \n {error}");
            Close();
        }

        /// <summary>
        /// Called once the Model, View and Controller are created and ready.
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
