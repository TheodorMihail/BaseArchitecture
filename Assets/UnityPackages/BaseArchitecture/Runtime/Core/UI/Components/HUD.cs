using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Persistent UI. Unlike a Screen, showing a HUD does not block, and it stays until closed.
    /// </summary>
    public interface IHUD : IUIComponent
    {
        const string HUDContainerID = "HUDContainer";
        const string HUDCategoryID = "HUD";
    }

    /// <summary>
    /// Base HUD class. Creates and owns the Model, View and Controller, loading the View's prefab
    /// from the address on its AddressablePathAttribute.
    /// </summary>
    public abstract class HUD<M, V, C> : UIComponent<M, V, C>, IHUD
        where M : IModel
        where V : IView
        where C : IController
    {
        [Inject] private readonly Transform _hudContainer;

        protected override Transform GetContainer() => _hudContainer;

        public override string UICategoryID => IHUD.HUDCategoryID;
        public override string UIContainerID => IHUD.HUDContainerID;
    }
}
