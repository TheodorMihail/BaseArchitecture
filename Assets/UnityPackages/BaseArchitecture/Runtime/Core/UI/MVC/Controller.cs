using System;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Controller in the MVC pattern. Initialized and disposed by the framework.
    /// </summary>
    public interface IController : IInitializable, IDisposable
    {
    }

    /// <summary>
    /// Base class for Controllers, which wire the View's events to the Model and back.
    /// Subscriptions go in Initialize() and are undone in Dispose().
    /// </summary>
    public abstract class Controller<S, M, V> : IController
        where S : IUIComponent
        where M : IModel
        where V : IView
    {
        protected readonly S _uiComponent;
        protected readonly M _model;
        protected readonly V _view;

        public Controller(S uiComponent, M model, V view)
        {
            _uiComponent = uiComponent;
            _model = model;
            _view = view;
        }

        public virtual void Initialize()
        {
            if (_view is View<M> viewWithModel)
            {
                viewWithModel.SetModel(_model);
            }

            _view.Initialize();
        }

        public virtual void Dispose()
        {
        }

        protected virtual void Close()
        {
            _uiComponent.Close();
        }
        
        protected virtual void CloseScreenWithResult<TResult>(TResult result) 
            where TResult : IScreenResult
        {
            if (_uiComponent is IScreenWithResult<TResult> screenWithResult)
            {
                screenWithResult.SetResult(result);
            }

            Close();
        }
    }
}
