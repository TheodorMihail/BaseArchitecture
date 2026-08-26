namespace BaseArchitecture.Core
{
    /// <summary>
    /// An object the pool can hand out and take back.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Called when the object is taken from the pool and activated. A pooled instance carries the
        /// state it had when it went back, so anything per-use is reset here.
        /// </summary>
        void OnSpawned();

        /// <summary>
        /// Called when the object is returned to the pool and deactivated. Event subscriptions are
        /// dropped here, since the instance outlives this use.
        /// </summary>
        void OnDespawned();
    }
}
