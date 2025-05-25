using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Base.Systems
{
    public interface IView : IInitializable
    {
        void CloseView();
    }

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
