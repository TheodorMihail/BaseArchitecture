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
        const string ScreenCategoryID = "Screen";

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
    /// Marker interface for screen result data structures.
    /// </summary>
    public interface IScreenResult { }

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

        public override string UICategoryID => IScreen.ScreenCategoryID;
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
}
