using System;
using UnityEngine;
using Zenject;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// View in the MVC pattern: presentation, raising events for the Controller to handle.
    /// </summary>
    public interface IView : IInitializable
    {
    }

    /// <summary>
    /// Base class for Views, which are MonoBehaviours instantiated from a prefab. Communication with
    /// the Controller is by event rather than direct calls.
    /// </summary>
    public abstract class View : MonoBehaviour, IView
    {
        public virtual void Initialize() { }
    }

    /// <summary>View that queries its Model on demand instead of being handed pushed values. The
    /// Controller sets the Model before Initialize() runs.</summary>
    public abstract class View<TModel> : View where TModel : IModel
    {
        protected TModel _model;

        public void SetModel(TModel model)
        {
            _model = model;
        }
    }

    /// <summary>
    /// The Addressables path a View's prefab is loaded from.
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
