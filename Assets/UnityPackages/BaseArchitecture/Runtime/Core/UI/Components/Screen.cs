using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// UI that can be awaited until it closes, which is how navigation is expressed.
    /// </summary>
    public interface IScreen : IUIComponent
    {
        const string ScreensContainerID = "ScreenContainer";
        const string ScreenCategoryID = "Screen";

        UniTask WaitForClosure();
    }

    /// <summary>
    /// Screen that hands back a result when it closes.
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
    /// Base Screen class. Creates and owns the Model, View and Controller, loading the View's prefab
    /// from the address on its AddressablePathAttribute, and can be awaited until it closes.
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
