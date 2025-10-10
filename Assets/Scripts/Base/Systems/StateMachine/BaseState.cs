using System;

namespace Base.Systems
{
    public interface IState<T> where T : Enum
    {
        T Id { get; }
        event Action<(T stateId, object[] paramsList)> OnStateFinished;
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    /// <summary>
    /// Base class for state machine states. Override OnEnter/OnUpdate/OnExit for state logic.
    /// Call FinishState() with optional parameters to signal completion and trigger state transitions.
    /// </summary>
    public abstract class BaseState<T> : IState<T>
        where T : Enum
    {
        public abstract T Id { get; }
        public event Action<(T stateId, object[] paramsList)> OnStateFinished;

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        /// <summary>
        /// Signals that this state has completed. Parameters are passed to the state machine's OnStateFinished handler.
        /// </summary>
        protected void FinishState(params object[] paramsList)
        {
            OnStateFinished.Invoke((Id, paramsList));
        }
    }
}