using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Interface for Screen components that manage the full MVC lifecycle.
    /// Screens can be awaited to handle async UI flows and state transitions.
    /// </summary>
    public interface IScreen : IUIComponent
    {
        const string ScreensContainerID = "ScreenContainer";
        const string ScreenComponentName = "Screen";

        UniTask WaitForClosure();
    }

    /// <summary>
    /// Screen interface that supports returning a result upon closure.
    /// </summary>
    public interface IScreenWithResult<TResult> : IScreen
        where TResult : IScreenResult
    {
        TResult GetResult();
        void SetResult(TResult result);
    }

    /// <summary>
    /// Screen interface that supports receiving typed parameters.
    /// Parameters are set before initialization.
    /// </summary>
    public interface IScreenWithParams<TParam> : IScreen
        where TParam : IScreenParam
    {
        void SetParameter(TParam parameter);
    }

    /// <summary>
    /// Marker interface for screen result data structures.
    /// </summary>
    public interface IScreenResult { }

    /// <summary>
    /// Marker interface for screen parameter data structures.
    /// </summary>
    public interface IScreenParam { }


    /// <summary>
    /// Base Screen class that automatically creates and manages Model, View, and Controller.
    /// View prefabs are loaded using the AddressablePathAttribute on the View class.
    /// Provides async/await support for handling UI flows and state transitions.
    /// </summary>
    public abstract class Screen<M, V, C> : UIComponent<M, V, C>, IScreen
        where M : IModel
        where V : IView
        where C : IController
    {
        [Inject] private readonly Transform _screenContainer;

        private UniTaskCompletionSource _screenClosedTcs = new UniTaskCompletionSource();

        protected override Transform GetContainer() => _screenContainer;
        protected override string GetComponentTypeName() => IScreen.ScreenComponentName;

        public override string UIContainerID => IScreen.ScreensContainerID;

        public UniTask WaitForClosure()
        {
            return _screenClosedTcs.Task;
        }

        public override void Close()
        {
            _screenClosedTcs.TrySetResult();
            base.Close();
        }
    }
    
    /// <summary>
    /// Extended Screen class that supports receiving typed parameters during initialization.
    /// </summary>
    public abstract class ScreenWithParams<M, V, C, TParam> : Screen<M, V, C>, IScreenWithParams<TParam>
        where M : IModel
        where V : IView
        where C : IController
        where TParam : IScreenParam
    {
        private TParam _parameter;
    
        public void SetParameter(TParam parameter) => _parameter = parameter;
    
        protected override void MVCCreated()
        {
            base.MVCCreated();
        
            if (_model is IModelWithParams<TParam> modelWithParams)
            {
                modelWithParams.InitializeWithParameters(_parameter);
            }
        }
    }   
}
