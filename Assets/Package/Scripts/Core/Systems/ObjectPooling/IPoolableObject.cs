namespace BaseArchitecture.Core
{
    /// <summary>
    /// Interface for objects that can be managed by an object pool.
    /// Implement this on MonoBehaviours that will be spawned/despawned frequently.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Called when the object is taken from the pool and made active.
        /// Use this to reset state, enable components, etc.
        /// </summary>
        void OnSpawned();

        /// <summary>
        /// Called when the object is returned to the pool and deactivated.
        /// Use this to clean up, disable components, unsubscribe from events, etc.
        /// </summary>
        void OnDespawned();
    }
}
