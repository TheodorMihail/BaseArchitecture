using System;
using Zenject;

namespace Base.Systems
{
    public interface IController : IInitializable, IDisposable
    {
    }

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

    public abstract class Controller<S, M, V, TResult> : Controller<S, M, V>
        where S : IScreen<TResult>
        where M : IModel
        where V : IView
        where TResult : IScreenResult
    {
        protected Controller(S screen, M model, V view) 
            : base(screen, model, view)
        {
        }

        public virtual void CloseScreen(TResult result)
        {
            _screen.SetResult(result);
            _screen.CloseScreen();
        }
    }
}
