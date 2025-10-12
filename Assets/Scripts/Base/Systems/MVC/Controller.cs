using System;
using Zenject;

namespace Base.Systems
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
        where S : IScreen
        where M : IModel
        where V : IView
    {
        protected readonly S _screen;
        protected readonly M _model;
        protected V _view;

        public Controller(S screen, M model, V view)
        {
            _screen = screen;
            _model = model;
            _view = view;
        }

        public virtual void Initialize()
        {
            _view.Initialize();
        }

        public virtual void Dispose()
        {
            _view.CloseView();
        }

        public virtual void CloseScreen()
        {
            _screen.CloseScreen();
        }
    }
}
