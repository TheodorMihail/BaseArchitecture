using System;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Interface for View components in the MVC pattern.
    /// Views handle UI presentation and forward user input to Controllers via events.
    /// </summary>
    public interface IView : IInitializable
    {
    }

    /// <summary>
    /// Base class for MVC Views that represent UI elements.
    /// Views are MonoBehaviours instantiated from prefabs and should contain minimal logic.
    /// Use events to communicate with Controllers rather than direct method calls.
    /// </summary>
    public abstract class View : MonoBehaviour, IView
    {
        public virtual void Initialize() { }
    }

    /// <summary>Opt-in View base for screens that query their Model on demand instead of receiving pushed data. Controller{S,M,V} sets the Model before Initialize() runs.</summary>
    public abstract class View<TModel> : View where TModel : IModel
    {
        protected TModel _model;

        public void SetModel(TModel model)
        {
            _model = model;
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
