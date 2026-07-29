using System;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Interface for Controller components in the MVC pattern.
    /// Controllers are initialized and disposed automatically by the framework.
    /// </summary>
    public interface IController : IInitializable, IDisposable
    {
    }

    /// <summary>
    /// Base class for Controllers that orchestrate Model and View interactions.
    /// Subscribe to View events in Initialize(), update Models based on user input,
    /// and update Views based on Model changes. Unsubscribe from events in Dispose().
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
