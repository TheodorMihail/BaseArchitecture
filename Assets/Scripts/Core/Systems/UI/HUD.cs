using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Interface for HUD components that manage persistent UI.
    /// Unlike Screens, HUDs don't block execution and remain visible until explicitly hidden.
    /// </summary>
    public interface IHUD : IUIComponent
    {
        const string HUDContainerID = "HUDContainer";
        const string HUDComponentName = "HUD";
    }

    /// <summary>
    /// Base HUD class that automatically creates and manages Model, View, and Controller.
    /// HUDs are persistent UI elements that don't block execution flow.
    /// View prefabs are loaded using the AddressablePathAttribute on the View class.
    /// </summary>
    public abstract class HUD<M, V, C> : UIComponent<M, V, C>, IHUD
        where M : IModel
        where V : IView
        where C : IController
    {
        [Inject] private readonly Transform _hudContainer;

        protected override Transform GetContainer() => _hudContainer;
        protected override string GetComponentTypeName() => IHUD.HUDComponentName;

        public override string UIContainerID => IHUD.HUDContainerID;
    }
}
