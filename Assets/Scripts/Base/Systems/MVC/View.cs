using System;
using UnityEngine;
using Zenject;

namespace Base.Systems
{
    /// <summary>
    /// Interface for View components in the MVC pattern.
    /// Views handle UI presentation and forward user input to Controllers via events.
    /// </summary>
    public interface IView : IInitializable
    {
        void CloseView();
    }

    /// <summary>
    /// Base class for MVC Views that represent UI elements.
    /// Views are MonoBehaviours instantiated from prefabs and should contain minimal logic.
    /// Use events to communicate with Controllers rather than direct method calls.
    /// </summary>
    public abstract class View : MonoBehaviour, IView
    {
        public bool HasBeenDestroyed => this == null || gameObject == null;

        public virtual void Initialize() { }

        public void CloseView()
        {
            if (!HasBeenDestroyed)
                GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// Attribute that specifies the Addressables path for a View prefab.
    /// Applied to View classes to enable automatic loading from Addressables.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AddressablePathAttribute : Attribute
    {
        public string Path { get; }

        public AddressablePathAttribute(string path)
        {
            Path = path;
        }
    }
}
